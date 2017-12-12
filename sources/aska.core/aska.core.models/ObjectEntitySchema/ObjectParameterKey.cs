using System;
using aska.core.models.ObjectEntitySchema.Security;

namespace aska.core.models.ObjectEntitySchema
{
    public enum ObjectParameterKey
    {
        Unknown = 0,

        [IsPublic]
        Name = 1,

        [IsPublic]
        Description = 2,

        [IsPublic]
        DisplayName = 3,

        [IsPublic]
        FullTitle = 4,

        [IsPublic]
        ShortTitle = 5,

        [IsPublic]
        LocationId = 6,
        
        [IsPublic]
        Floor = 7,

        [IsPublic]
        FloorsTotal = 8,

        [IsPublic]
        BuildingType = 9,

        [IsPublic]
        AreaTotal = 10,

        [IsPublic]
        AreaLiving = 11,

        [IsPublic]
        FinishingType = 12,

        [IsPublic]
        CostTotal = 13,

        [IsPublic]
        CostPerMeter = 14,

        [IsPublic]
        TitleImageId = 15,

        [IsPublic]
        AddressLine1 = 16,

        [IsPublic]
        AddressLine2 = 17,

        [IsPublic]
        AddressRegion = 18,

        [IsPublic]
        PlanImageId = 19,

        [IsPublic]
        ViewImageId = 20,

        [IsPublic]
        DocumentAttachmentId = 21,

        [IsPublic]
        AddressCoordinates = 22,

        [IsPublic]
        ParkingType = 23,

        [IsPublic]
        HotOffer = 24,

        /// <summary>
        /// Площадь земельного уччастка, сотки
        /// </summary>
        [IsPublic]
        LandArea = 25,

        /// <summary>
        /// Ширина фасада участка
        /// </summary>
        [IsPublic]
        FacadeWidth = 26,

        /// <summary>
        /// Назначение земли
        /// </summary>
        [IsPublic]
        LandPurpose = 27,

        /// <summary>
        /// Цена за сотку земли
        /// </summary>
        [IsPublic]
        CostPerHundredSquareMetres = 28,

        /// <summary>
        /// Городские коммуникации
        /// </summary>
        [IsPublic]
        MunicipalServices = 29,

        /// <summary>
        /// ширина улиц
        /// </summary>
        [IsPublic]
        StreetWidth = 30,

        /// <summary>
        /// Наличие цоколя
        /// </summary>
        [IsPublic]
        HasGroundFloor = 31,

        [IsPublic]
        BuildingMaterials = 32,

        [IsPublic]
        BuildingCondition = 33,

        /// <summary>
        /// Аннотация
        /// </summary>
        [IsPublic]
        Intro = 34,

        [IsPublic]
        OverviewImageId = 35,

        
        ManagerPhone = 36,

        /// <summary>
        /// Название для ссылок - латиница, без пробелов
        /// </summary>
        [IsPublic]
        Alias = 37,

        /// <summary>
        /// ID элемента в старой схеме БД
        /// </summary>
        LegacyId = 38,

        /// <summary>
        /// Количество литеров
        /// </summary>
        [IsPublic]
        BuildingsTotal = 39,

        /// <summary>
        /// Высота потолков
        /// </summary>
        [IsPublic]
        CeilingHeight = 40,

        /// <summary>
        /// Срок сдачи
        /// </summary>
        [IsPublic]
        Deadline = 41,

        /// <summary>
        /// Link to ObjectEntity[type=BrandEntityId]
        /// </summary>
        [IsPublic]
        BrandEntityId = 42,

        [IsPublic]
        Order = 43,

        /// <summary>
        /// Изображение генплана
        /// </summary>
        [IsPublic]
        MasterPlanImageId = 44,

        /// <summary>
        /// Статус объекта - на сдаче, сдан, в проекте....
        /// </summary>
        [IsPublic]
        Status = 45,

        /// <summary>
        /// Дата события
        /// </summary>
        [IsPublic]
        OccasionDate = 46,

        /// <summary>
        /// На каких сайтах показывать элемент
        /// </summary>
        TargetSite = 47,

        /// <summary>
        /// Ссылка на внешний ресурс
        /// </summary>
        [IsPublic]
        ExternalUrl = 48,

        /// <summary>
        /// Ссылка на youtube
        /// </summary>
        [IsPublic]
        YoutubeEmbeddedUrl = 49,

        /// <summary>
        /// Link to ObjectEntity[type=PhotoReport]
        /// </summary>
        [IsPublic]
        PhotoreportEntityId = 50,

        /// <summary>
        /// Родительский элемент при переводе консьюмера на следующий уровень (contact = > lead, lead => client,...)
        /// </summary>
        ParentConsumerEntityId = 51,

        /// <summary>
        /// Статус записи клиента - ожидает, в работе, завершено, ...
        /// </summary>
        ConsumerState = 52,

        /// <summary>
        /// Ссылка на запись взаимодействия
        /// </summary>
        ConsumerInteractionEntityId = 53,

        /// <summary>
        /// Канал взаимодействия
        /// </summary>
        ConsumerInteractionChannelEntityId = 54,

        /// <summary>
        /// Вид взаимодействия с клиентом
        /// </summary>
        ConsumerInteractionType = 55,

        Token = 56,

        UTM = 57,

        ClientIpAddress = 58,
        ClientUserAgent = 59,
        ClientGeoLocation = 60,

        [IsPublic]
        CompanyTitle = 61,
        
        ManagerName = 62,

        
        Phone = 63,

        
        Email = 64,


        [IsPublic]
        VotePublicScore = 65,

        
        VotePrivateScore = 66,

        /// <summary>
        /// Должность
        /// </summary>
        Position = 67,

        AccountDataProfile = 68,

        Identifier = 69,
        TokenEpoch = 70,

        Message = 71,
        Subject = 72,

        [IsPublic]
        DevelopmentType = 73,

        [IsPublic]
        LandCadastralNumber = 74,

        [IsPublic]
        ProjectDescription = 75,

        [IsPublic]
        IsPromotional = 76,

        LeadSourceName = 77,

        PbxCallDirection = 78,
        PbxCallRecordId = 79,
        PbxAbonentId = 80,
        [Obsolete("Replaced with user principal IDs collection")]
        InFavouritesObsolete = 81,
        InFavourites = 82,

    }
}