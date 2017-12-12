namespace aska.core.models.ObjectEntitySchema
{
    public enum ObjectParameterCategory
    {
        [Title("Общие данные")]
        Common = 0,

        [Title("Расположение")]
        Location = 1,

        [Title("Изображения")]
        Images = 2,

        [Title("Документы")]
        Documents = 3,

        [Title("Промо")]
        Promotional = 4,

        [Title("Цены")]
        Price = 5,

        [Title("Приватные данные")]
        Private = 6,

        
        Internal = 255
    }
}