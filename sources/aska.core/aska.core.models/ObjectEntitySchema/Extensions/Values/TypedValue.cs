using System;
using aska.core.models.Variants;

namespace aska.core.models.ObjectEntitySchema.Extensions.Values
{
    public static class TypedValue
    {
        public static string AsRaw(this ObjectParameterValue rawValue)
        {
            return rawValue != null ? rawValue.RawValue : null;
        }

        public static dynamic AsTyped(this ObjectParameterValue rawValue, dynamic defaultValue)
        {
            if (rawValue == null) return defaultValue;

            switch (rawValue.ParameterKey)
            {
                // string
                case ObjectParameterKey.Unknown:
                case ObjectParameterKey.Name:
                case ObjectParameterKey.Description:
                case ObjectParameterKey.DisplayName:
                case ObjectParameterKey.FullTitle:
                case ObjectParameterKey.ShortTitle:
                case ObjectParameterKey.AddressLine1:
                case ObjectParameterKey.AddressLine2:
                case ObjectParameterKey.AddressRegion:
                case ObjectParameterKey.AddressCoordinates:
                case ObjectParameterKey.Intro:
                case ObjectParameterKey.ManagerPhone:
                case ObjectParameterKey.Alias:
                case ObjectParameterKey.Deadline:
                case ObjectParameterKey.Status:
                case ObjectParameterKey.Token:
                case ObjectParameterKey.UTM:
                case ObjectParameterKey.ClientIpAddress:
                case ObjectParameterKey.ClientUserAgent:
                case ObjectParameterKey.ClientGeoLocation:
                case ObjectParameterKey.AccountDataProfile:
                case ObjectParameterKey.Position:
                case ObjectParameterKey.CompanyTitle:
                case ObjectParameterKey.ManagerName:
                case ObjectParameterKey.Phone:
                case ObjectParameterKey.Email:
                case ObjectParameterKey.Identifier:
                case ObjectParameterKey.Message:
                case ObjectParameterKey.Subject:
                case ObjectParameterKey.LandCadastralNumber:
                case ObjectParameterKey.ProjectDescription:
                case ObjectParameterKey.LeadSourceName:
                case ObjectParameterKey.PbxAbonentId:
                    return rawValue.RawValue;

                // bool
                case ObjectParameterKey.HotOffer:
                case ObjectParameterKey.HasGroundFloor:
                case ObjectParameterKey.IsPromotional:
                case ObjectParameterKey.InFavouritesObsolete:
                    return AsBool(rawValue.RawValue, defaultValue);

                // datetimne
                case ObjectParameterKey.OccasionDate:
                    return AsDateTime(rawValue.RawValue, defaultValue);

                // uri
                case ObjectParameterKey.ExternalUrl:
                case ObjectParameterKey.YoutubeEmbeddedUrl:
                    return AsUri(rawValue.RawValue, defaultValue);

                //int
                case ObjectParameterKey.Floor:
                case ObjectParameterKey.FloorsTotal:
                case ObjectParameterKey.BuildingsTotal:
                case ObjectParameterKey.Order:
                case ObjectParameterKey.VotePublicScore:
                case ObjectParameterKey.VotePrivateScore:
                case ObjectParameterKey.TokenEpoch:
                    return AsInt(rawValue.RawValue, defaultValue);

                // decimal
                case ObjectParameterKey.AreaTotal:
                case ObjectParameterKey.AreaLiving:
                case ObjectParameterKey.CostTotal:
                case ObjectParameterKey.CostPerMeter:
                case ObjectParameterKey.LandArea:
                case ObjectParameterKey.FacadeWidth:
                case ObjectParameterKey.CostPerHundredSquareMetres:
                case ObjectParameterKey.StreetWidth:
                case ObjectParameterKey.CeilingHeight:
                    return  AsDecimal(rawValue.RawValue, defaultValue);


                // guid
                case ObjectParameterKey.TitleImageId:
                case ObjectParameterKey.LocationId:
                case ObjectParameterKey.PlanImageId:
                case ObjectParameterKey.ViewImageId:
                case ObjectParameterKey.DocumentAttachmentId:
                case ObjectParameterKey.OverviewImageId:
                case ObjectParameterKey.LegacyId:
                case ObjectParameterKey.BrandEntityId:
                case ObjectParameterKey.MasterPlanImageId:
                case ObjectParameterKey.PhotoreportEntityId:
                case ObjectParameterKey.ParentConsumerEntityId:
                case ObjectParameterKey.ConsumerInteractionEntityId:
                case ObjectParameterKey.ConsumerInteractionChannelEntityId:
                case ObjectParameterKey.PbxCallRecordId:
                case ObjectParameterKey.InFavourites:
                    return  AsGuid(rawValue.RawValue, defaultValue);

                // enums
                case ObjectParameterKey.BuildingType:
                    return AsEnum<BuildingType>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.FinishingType:
                    return AsEnum<FinishingType>(rawValue.RawValue, defaultValue);
                    
                case ObjectParameterKey.ParkingType:
                    return AsEnum<ParkingType>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.LandPurpose:
                    return AsEnum<LandPurpose>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.MunicipalServices:
                    return AsEnum<MunicipalService>(rawValue.RawValue, defaultValue);
                    
                case ObjectParameterKey.BuildingMaterials:
                    return AsEnum<BuildingMaterials>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.BuildingCondition:
                    return AsEnum<BuildingCondition>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.TargetSite:
                    return AsEnum<SiteProfileKey>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.ConsumerState:
                    return AsEnum<ConsumerState>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.ConsumerInteractionType:
                    return AsEnum<ConsumerInteractionType>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.DevelopmentType:
                    return AsEnum<DevelopmentType>(rawValue.RawValue, defaultValue);

                case ObjectParameterKey.PbxCallDirection:
                    return AsEnum<PbxCallDirection>(rawValue.RawValue, defaultValue);

                default:
                    throw new ArgumentOutOfRangeException(nameof(rawValue), "Not implemented type conversion for parameter key: " + rawValue.ParameterKey.ToString("G"));
            }
        }
        
        private static T AsEnum<T>(string raw, T defaultValue) where T : struct
        {
            T value;
            return Enum.TryParse(raw, true, out value) ? value : defaultValue;
        }

        private static Guid AsGuid(string raw, Guid defaultValue)
        {
            Guid value;
            return Guid.TryParse(raw, out value) ? value : defaultValue;
        }

        private static decimal AsDecimal(string raw, decimal defaultValue)
        {
            decimal value;
            return decimal.TryParse(raw, out value)? value: defaultValue;
        }

        private static bool AsBool(string raw, bool defaultValue)
        {
            bool value;
            return bool.TryParse(raw, out value) ? value : defaultValue;
        }

        private static int AsInt(string raw, int defaultValue)
        {
            int value;
            return int.TryParse(raw, out value) ? value : defaultValue;
        }

        private static DateTime AsDateTime(string raw, DateTime defaultValue)
        {
            DateTime value;
            return DateTime.TryParse(raw, out value)? value : defaultValue;
        }

        private static Uri AsUri(string raw, Uri defaultValue)
        {
            Uri result = null;
            return Uri.TryCreate(raw, UriKind.Absolute,  out result) ? result : defaultValue;
        }
    }
}