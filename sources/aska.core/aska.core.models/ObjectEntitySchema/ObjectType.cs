using System;
using aska.core.models.ObjectEntitySchema.Security;
using aska.core.models.Variants;

namespace aska.core.models.ObjectEntitySchema
{
    public enum ObjectType
    {
        [ViewPermission(UserClaim.CanViewEntities)]
        [EditPermission(UserClaim.CanEditEntities)]
        [Title("Запись для внутреннего использования")]
        Internal = 9999,

        [Title("-")]
        Unknown = 0,

        [Title("Объект недвижимости")]
        [Obsolete("Replaced with three new types because of different params set", true)]
        RealtyObject = 1,

        [Title("Бренд")]
        [ViewPermission(UserClaim.CanViewBrands)]
        [EditPermission(UserClaim.CanEditBrands)]
        [IsPublic]
        Brand = 2,

        [Title("Сотрудник")]
        [Obsolete("Not used.", true)]
        Employee = 3,

        //
        //      RESALE
        [ViewPermission(UserClaim.CanViewResale)]
        [EditPermission(UserClaim.CanEditResale)]
        [IsPublic]
        [Title("Вторичка - Квартира")]
        ResaleApartment = 4,


        [ViewPermission(UserClaim.CanViewResale)]
        [EditPermission(UserClaim.CanEditResale)]
        [IsPublic]
        [Title("Вторичка - Дом")]
        ResaleHouse = 5,

        [ViewPermission(UserClaim.CanViewResale)]
        [EditPermission(UserClaim.CanEditResale)]
        [IsPublic]
        [Title("Вторичка - Земельный участок")]
        ResaleLand = 6,


        //      REALTY
        [ViewPermission(UserClaim.CanViewRealty)]
        [EditPermission(UserClaim.CanEditRealty)]
        [IsPublic]
        [Title("Недвижимость - Жилые комплексы, квартиры")]
        RealtyApartments = 7,

        [ViewPermission(UserClaim.CanViewRealty)]
        [EditPermission(UserClaim.CanEditRealty)]
        [IsPublic]
        [Title("Недвижимость - Коттеджные поселки")]
        RealtyCottages = 8,

        [ViewPermission(UserClaim.CanViewRealty)]
        [EditPermission(UserClaim.CanEditRealty)]
        [IsPublic]
        [Title("Недвижимость - Земля")]
        RealtyLand = 9,

        //      EVENT
        [ViewPermission(UserClaim.CanViewEvents)]
        [EditPermission(UserClaim.CanEditEvents)]
        [IsPublic]
        [Title("События - Новости (текст)")]
        EventNewsText = 10,

        [ViewPermission(UserClaim.CanViewEvents)]
        [EditPermission(UserClaim.CanEditEvents)]
        [IsPublic]
        [Title("События - Акции")]
        EventPromo = 11,

        [ViewPermission(UserClaim.CanViewEvents)]
        [EditPermission(UserClaim.CanEditEvents)]
        [IsPublic]
        [Title("События - Новости (видео)")]
        EventNewsVideo = 12,

        //      PHOTOS
        [ViewPermission(UserClaim.CanViewRealty)]
        [EditPermission(UserClaim.CanEditRealty)]
        [IsPublic]
        [Title("Фото - фотоотчет")]
        PhotoReport = 13,

        [ViewPermission(UserClaim.CanViewPhotofeed)]
        [EditPermission(UserClaim.CanEditPhotofeed)]
        [IsPublic]
        [Title("Фото - лента")]
        PhotoFeed = 14,


        //
        //      Consumer interaction
        [ViewPermission(UserClaim.CanViewConsumers)]
        [EditPermission(UserClaim.CanEditConsumers)]
        [Title("Клиент - Интерес")]
        ConsumerInterest = 15,

        [ViewPermission(UserClaim.CanViewConsumers)]
        [EditPermission(UserClaim.CanEditConsumers)]
        [Title("Клиент - Контакт")]
        ConsumerContact = 16,

        [ViewPermission(UserClaim.CanViewConsumers)]
        [EditPermission(UserClaim.CanEditConsumers)]
        [Title("Клиент - Лид")]
        ConsumerLead = 17,

        [ViewPermission(UserClaim.CanViewConsumers)]
        [EditPermission(UserClaim.CanEditConsumers)]
        [Title("Клиент - Клиент")]
        ConsumerClient = 18,



        //      interaction
        [ViewPermission(UserClaim.CanViewConsumerInteractionChannels)]
        [EditPermission(UserClaim.CanEditConsumerInteractionChannels)]
        [Title("Клиент - Канал взаимодействия")]
        InteractionChannel = 19,


        [ViewPermission(UserClaim.CanViewConsumerInteractionOccasion)]
        [EditPermission(UserClaim.CanEditConsumerInteractionOccasion)]
        [Title("Событие взаимодействия - без типа")]
        InteractionOccasion = 20,

        // SecretMission campaign
        // voting
        [ViewPermission(UserClaim.CanViewAccounts)]
        [EditPermission(UserClaim.CanEditAccounts)]
        [Title("Пользователь - Риэлтор")]
        [IsPublic] //public voting - secret mission 2
        UserDataRealtor = 21,

        [ViewPermission(UserClaim.CanViewAccounts)]
        [EditPermission(UserClaim.CanEditAccounts)]
        [Title("Пользователь - Сотрудник")]
        UserDataEmployee = 22,


        // notifications
        [ViewPermission(UserClaim.CanViewNotifications)]
        [EditPermission(UserClaim.CanEditNotifications)]
        [Title("Уведомление для пользователей")]
        NotificationInternal = 23,

        [ViewPermission(UserClaim.CanViewRealtyBoard)]
        [EditPermission(UserClaim.CanEditRealtyBoard)]
        [Title("Шахматка")]
        RealtyBoard = 24,


        [ViewPermission(UserClaim.CanViewAvaThanksShop)]
        [EditPermission(UserClaim.CanEditAvaThanksShop)]
        [Title("Приз - АВА-Комплимент")]
        AvaThanksBonus = 25,


        [ViewPermission(UserClaim.CanViewConsumerInteractionOccasion)]
        [EditPermission(UserClaim.CanEditConsumerInteractionOccasion)]
        [Title("Событие взаимодействия - Запрос обратного звонка")]
        InteractionOccasion_CallbackRequest = 26,


        [ViewPermission(UserClaim.CanViewVacancy)]
        [EditPermission(UserClaim.CanEditVacancy)]
        [Title("Вакансия")]
        [IsPublic]
        Vacancy = 27,

        [ViewPermission(UserClaim.CanViewConsumerInteractionOccasion)]
        [EditPermission(UserClaim.CanEditConsumerInteractionOccasion)]
        [Title("Событие взаимодействия - Запрос обратного звонка")]
        InteractionOccasion_FeedbackForm = 28,


        [ViewPermission(UserClaim.CanViewConsumerInteractionOccasion)]
        [EditPermission(UserClaim.CanEditConsumerInteractionOccasion)]
        [IsPublic]
        [Title("Объект инвестиций")]
        InvestmentObject = 29,

        [Title("Строительная компания")]
        [ViewPermission(UserClaim.CanViewBrands)]
        [EditPermission(UserClaim.CanEditBrands)]
        [IsPublic]
        ConstructionCompany = 30,


        [ViewPermission(UserClaim.CanViewConsumerInteractionOccasion)]
        [EditPermission(UserClaim.CanEditConsumerInteractionOccasion)]
        [Title("Событие взаимодействия - Запись звонка")]
        InteractionOccasion_CallRecord = 31,

        [ViewPermission(UserClaim.CanViewAccounts)]
        [EditPermission(UserClaim.CanEditAccounts)]
        [Title("Пользователь - Руководитель отдела продаж")]
        UserDataSalesLead = 32,

    }
}