using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace DatasetTest
{
    //Add DesignerCategory attribute to stop VS from treating this class as a component
    [System.ComponentModel.DesignerCategory("Code")]
    public class TypedTableBaseEx<T> : System.Data.TypedTableBase<T> where T : System.Data.DataRow
    {
        public TypedTableBaseEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
           
        }
        public TypedTableBaseEx() { }
        public string TestProp { get; set; }
    }
}
