using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using kd.domainmodel.Attachment;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.infrastructure.TaskScheduler;
using kd.services.attachment.Converter.AttachmentConverter;
using kd.services.attachment.Converter.ConversionTaskFactory;
using NLog;
using Quartz;

namespace kd.services.attachment.Converter
{
    [DisallowConcurrentExecution]
    public class AttachmentConverterJob : IJob, IStartupJobScheduler
    {
        public const string Identity = "attachment-converter-job";
#if DEBUG
        public const byte MaxParallelConversionTasks = 1;
#else
        public const byte MaxParallelConversionTasks = 2;
#endif

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;
        private readonly IIndex<ObjectAttachmentVariant, IAttachmentConverter> _converters;

        public AttachmentConverterJob(
            IQueryFactory queryFactory, 
            ICommandFactory commandFactory,
            IIndex<ObjectAttachmentVariant, IAttachmentConverter> converters)
        {
            _queryFactory = queryFactory;
            _commandFactory = commandFactory;
            _converters = converters;
        }

        bool IStartupJobScheduler.Schedule(IScheduler scheduler)
        {
            var job = JobBuilder.Create<AttachmentConverterJob>()
                .WithIdentity(Identity)
                .Build();

            var trigger = TriggerBuilder.Create()
#if DEBUG
                .StartNow()
                //.StartAt(DateBuilder.FutureDate(1, IntervalUnit.Hour))
#else
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))
#endif
                .WithSimpleSchedule(s => s.WithIntervalInMinutes(5).RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
            Logger.Debug(" - Scheduled job '{0}'", Identity);
            return true;
        }
        

        Task IJob.Execute(IJobExecutionContext context)
        {
            try
            {
                var pendingTasks = _queryFactory.GetQuery<AttachmentConversionTask>()
                    .Where(AttachmentConversionTask.PendingRule)
                    .OrderBy(x => x.ResultVariant) // converting small thumbnails first
                    .All()
                    .ToList();
                if (!pendingTasks.Any())
                {
                    Logger.Debug("{0}: no pending tasks were found.", Identity);
                    return Task.CompletedTask;
                }

                Logger.Debug("{0}: found {1} pending tasks", Identity, pendingTasks.Count);
                Parallel.ForEach(
                    pendingTasks,
                    new ParallelOptions { MaxDegreeOfParallelism = MaxParallelConversionTasks },
                    Proceed
                );

                Logger.Debug("{0}: DONE processing tasks", Identity);
                foreach (var task in pendingTasks) _commandFactory.GetUpdateCommand<AttachmentConversionTask>().Execute(task);
                Logger.Debug("{0}: saved results for {1} tasks", Identity, pendingTasks.Count);

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error on converting attachments.");
                throw new JobExecutionException(e);
            }
        }


        private void Proceed(AttachmentConversionTask task)
        {
            try
            {
                task.ExecutionStartDateTimeUtc = DateTime.UtcNow;
                var converter = _converters[task.ResultVariant];

                Logger.Debug("{0} | #{1}: starting conversion to variant {2} with converter {3}", Identity, task.Id, task.ResultVariant, converter.GetType().Name);
                var result = converter.Proceed(task.ResultVariant, task.SourceFilePath, task.ResultFilePath);

                Logger.Log(result.Success ? LogLevel.Debug : LogLevel.Warn, "{0} | #{1}: done with status '{2}'. {3}", Identity, task.Id, result.Success, result.OperationLog);

                task.ExecutionEndDateTimeUtc = DateTime.UtcNow;
                task.Completed = true;
                task.Succeed = result.Success;
                task.OperationLog = result.OperationLog;
            }
            catch (Exception e)
            {
                Logger.Error(e, "{0} | #{1}: error while converting - {2}", Identity, task.Id, e.Message);
                task.OperationLog += "\n\r" + e.Message;
            }
        }
    }
}