using System;
using System.Collections.Generic;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public interface IPropertiesProvider
    {
        void ApplySimpleProperties();
        void ApplyCollections();
        Dictionary<string, IControlProperty> GetSimpleProperties();
        Dictionary<string, CollectionProperty> GetCollections();
        void LoadProperties(dynamic value);
        Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors();
        Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules();
    }
}