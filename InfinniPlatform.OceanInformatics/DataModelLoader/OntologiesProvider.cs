using System;
using System.Collections.Generic;
using System.Linq;
using OpenEhr.V1.Its.Xml.AM;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    internal sealed class OntologiesProvider
    {
        private readonly IDictionary<string, Tuple<string, string>> _ontologies;
        
        internal OntologiesProvider(IEnumerable<ARCHETYPE_TERM> terms)
        {
            _ontologies = new Dictionary<string, Tuple<string, string>>();

            foreach (var defItem in terms)
            {
                var description = (defItem.items.FirstOrDefault(i => i.id == "description") ?? defItem.items[0]).Value;
                var text = (defItem.items.FirstOrDefault(i => i.id == "text") ?? defItem.items[1]).Value;
                _ontologies.Add(defItem.code, new Tuple<string, string>(text, description));
            }
        }

        internal string GetText(string ontologyId)
        {
            return _ontologies.ContainsKey(ontologyId) ? _ontologies[ontologyId].Item1 : "Text: " + ontologyId;
        }

        internal string GetDescription(string ontologyId)
        {
            return _ontologies.ContainsKey(ontologyId) ? _ontologies[ontologyId].Item2 : "Description: " + ontologyId;
        }
    }
}