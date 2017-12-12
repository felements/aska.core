using System.Linq;

namespace aska.core.models.ObjectEntitySchema.Extensions.Values
{
    public partial class ObjectEntity
    {
        /// <summary>
        /// Operates only with first occurrence of parameterValue and only with RAW value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[ObjectParameterKey key]
        {
            get { return this.With(x=>x.Values).With(vals=>vals.FirstOrDefault(x => x.ParameterKey == key)).Let(v=>v.RawValue, null); }
            set
            {
                var valueEntity = Enumerable.FirstOrDefault<ObjectParameterValue>(this.Values, x => x.ParameterKey == key);
                if (valueEntity == null)
                {
                    valueEntity = new ObjectParameterValue(key, this.Id);
                    Values.Add(valueEntity);
                }
                valueEntity.RawValue = value;
            }
        }


    }
}