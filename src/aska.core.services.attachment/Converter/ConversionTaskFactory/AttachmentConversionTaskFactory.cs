using System;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.infrastructure.CommandQuery.Specification;
using NLog;

namespace kd.services.attachment.Converter.ConversionTaskFactory
{
    internal class AttachmentConversionTaskFactory : IAttachmentConversionTaskFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICommandFactory _commandFactory;
        private readonly IQueryFactory _queryFactory;

        public AttachmentConversionTaskFactory(ICommandFactory commandFactory, IQueryFactory queryFactory)
        {
            _commandFactory = commandFactory;
            _queryFactory = queryFactory;
        }

        /// <summary>
        /// Post to the operation queue new conversion task
        /// </summary>
        /// <param name="task"></param>
        public void Post(AttachmentConversionTask task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            _commandFactory.GetCreateCommand<AttachmentConversionTask>().Execute(task);
            Logger.Debug("Posted new attachment conversion task #{0} with variant '{1}' for file '{2}'", task.Id, task.ResultVariant, task.SourceFilePath);
        }

        /// <summary>
        /// duplicate the existing task
        /// </summary>
        /// <param name="taskId"></param>
        public void Rerun(Guid taskId)
        {
            var taskEntity = _queryFactory.GetQuery<AttachmentConversionTask, ByIdExpressionSpecification<AttachmentConversionTask>>()
                .Where(new ByIdExpressionSpecification<AttachmentConversionTask>(taskId))
                .SingleOrDefault();

            if (!taskEntity.Completed) return; //not required

            _commandFactory.GetCreateCommand<AttachmentConversionTask>().Execute(new AttachmentConversionTask(taskEntity.ResultVariant, taskEntity.SourceFilePath, taskEntity.ResultFilePath));
            Logger.Debug("Recreated attachment conversion task #{0} with variant '{1}' for file '{2}'", taskEntity.Id, taskEntity.ResultVariant, taskEntity.SourceFilePath);
        }
    }
}