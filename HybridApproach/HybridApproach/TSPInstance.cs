using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HybridApproach
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class travellingSalesmanProblemInstance
    {

        private string nameField;

        private string sourceField;

        private string descriptionField;

        private byte doublePrecisionField;

        private byte ignoredDigitsField;

        private travellingSalesmanProblemInstanceVertexEdge[][] graphField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public byte doublePrecision
        {
            get
            {
                return this.doublePrecisionField;
            }
            set
            {
                this.doublePrecisionField = value;
            }
        }

        /// <remarks/>
        public byte ignoredDigits
        {
            get
            {
                return this.ignoredDigitsField;
            }
            set
            {
                this.ignoredDigitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("vertex", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("edge", IsNullable = false, NestingLevel = 1)]
        public travellingSalesmanProblemInstanceVertexEdge[][] graph
        {
            get
            {
                return this.graphField;
            }
            set
            {
                this.graphField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class travellingSalesmanProblemInstanceVertexEdge
    {

        private float costField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float cost
        {
            get
            {
                return this.costField;
            }
            set
            {
                this.costField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
