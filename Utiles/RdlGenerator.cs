using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Utiles
{
   public class RdlGenerator
    {
        private List<string> m_allFields;
        private List<string> m_selectedFields;

        public List<string> AllFields
        {
            get { return m_allFields; }
            set { m_allFields = value; }
        }

        public List<string> SelectedFields
        {
            get { return m_selectedFields; }
            set { m_selectedFields = value; }
        }

        private Rdl.Report CreateReport()
        {
            Rdl.Report report = new Rdl.Report();
            report.Items = new object[]
                {
                    CreateDataSources(),
                    CreateBody(),
                    CreateHeaderFooter(),
                    CreateDataSets(),
                    "6.5in",
                };
            report.ItemsElementName = new Rdl.ItemsChoiceType37[]
                {
                    Rdl.ItemsChoiceType37.DataSources,
                    Rdl.ItemsChoiceType37.Body,
                     Rdl.ItemsChoiceType37.PageHeader,
                    Rdl.ItemsChoiceType37.DataSets,
                    Rdl.ItemsChoiceType37.Width,
                };
            return report;
        }
        private Rdl.PageHeaderFooterType CreateHeaderFooter()
        {
            Rdl.PageHeaderFooterType header = new Rdl.PageHeaderFooterType();
            header.Items = new object[]
            {
"4.38078cm",
CreateItems(),
true,
true,

            };
            header.ItemsElementName = new Rdl.ItemsChoiceType34[]
            {

Rdl.ItemsChoiceType34.Height,
Rdl.ItemsChoiceType34.ReportItems,
Rdl.ItemsChoiceType34.PrintOnFirstPage,
Rdl.ItemsChoiceType34.PrintOnLastPage,
            };
            return header;
        }
        public Rdl.ReportItemsType CreateItems()
        {
            Rdl.ReportItemsType items = new Rdl.ReportItemsType();
            Rdl.ReportItemsType items2 = new Rdl.ReportItemsType();
            items.Items = new Rdl.TextboxType[1];

            //this data will come from DB
            items.Items[0] = CreateTableCellTextbox("Control Articulo lote", true, 1);
            // items.Items[1] = CreateTableCellTextbox("CONTROL TEMP. SALIDA TUNEL", false, 2);
            //items.Items[2] = CreateTableCellTextbox("Printed on: 05/11/2016", false, 3);
            items2.Items = new object[]
                 {
                     CreateTableCellTextbox("Control Articulo lote", true, 1),
                    CreateHeaderTableCellRectangle ("Firma:",true,2),
                     };
            /*   items2.ItemsElementName = new Rdl.ItemsChoiceType27[]
               {
                   Rdl.TextboxType,
                   Rdl.RectangleType,

               };*/
            return items2;
        }

        private Rdl.RectangleType CreateHeaderTableCellRectangle(string fieldName, bool flag, int num)
        {
            Rdl.ReportItemsType items = new Rdl.ReportItemsType();
            items.Items = new Rdl.TextboxType[1];
            items.Items[0] = CreateTableCellTextbox("Firma: ", true, 2);
            /* Rdl.StyleType stilo = CreateRectangleStyle();
             Rdl.StyleType stilo2 = CreateTextboxStyle(num);*/
            Rdl.RectangleType headerTableCellRectangle = new Rdl.RectangleType();
            headerTableCellRectangle.Name = "rectangle" + num;// fieldName + "_Header";
                                                              // headerTableCellRectangle.Items =

            headerTableCellRectangle.Items = new object[]
            {
                "16.39913cm",
                "1.2827cm",
                "5.26303cm",
                CreateRectangleStyle(),
                "2.59748cm",
               items,
        };
            headerTableCellRectangle.ItemsElementName = new Rdl.ItemsChoiceType12[]
            {
                Rdl.ItemsChoiceType12.Left,
                Rdl.ItemsChoiceType12.Top,
                Rdl.ItemsChoiceType12.Width,
                Rdl.ItemsChoiceType12.Style,
                Rdl.ItemsChoiceType12.Height,
                Rdl.ItemsChoiceType12.ReportItems,

            };
            return headerTableCellRectangle;
        }
        Rdl.BorderColorStyleWidthType CreateBorderColorStyleWidth(string s)
        {
            Rdl.BorderColorStyleWidthType b = new Rdl.BorderColorStyleWidthType();
            b.Items = new object[]
                 {
                     s,
                 };
            b.ItemsElementName = new Rdl.ItemsChoiceType3[]
                 {
                     Rdl.ItemsChoiceType3.Default,
                 };
            return b;
        }

        private Rdl.StyleType CreateRectangleStyle()
        {
            Rdl.StyleType style = new Rdl.StyleType();


            style.Items = new object[]
            {
                    CreateBorderColorStyleWidth("black"),
                     CreateBorderColorStyleWidth("Solid"),
                      CreateBorderColorStyleWidth("2pt"),
            };

            style.ItemsElementName = new Rdl.ItemsChoiceType5[]
            {

                   Rdl.ItemsChoiceType5.BorderColor,
                     Rdl.ItemsChoiceType5.BorderStyle,
                     Rdl.ItemsChoiceType5.BorderWidth,
            };
            return style;
        }

        private Rdl.TextboxType CreateTableCellTextbox(string fieldName, bool flag, int num)
        {
            Rdl.TextboxType textbox = new Rdl.TextboxType();

            textbox.Name = "txt" + num;
            if (num == 1)
            {
                textbox.Items = new object[]
                {
fieldName ,"17.72799cm","1.26187cm","3.93417cm","2pt",
CreateTextboxStyle(num),
true,
                };
            }
            else if (num == 2)
            {
                textbox.Items = new object[]
                {
fieldName ,"1.91792cm","0.52062cm","1.62688cm","2pt",
CreateTextboxStyle(num),
true,
                };
            }
            else if (num == 3)
            {
                textbox.Items = new object[]
                {
fieldName ,"515pt","515pt","395pt","45pt",
CreateTextboxStyle(num),
true,
                };
            }
            textbox.ItemsElementName = new Rdl.ItemsChoiceType14[]
            {
Rdl.ItemsChoiceType14.Value,
Rdl.ItemsChoiceType14.Width,
Rdl.ItemsChoiceType14.Height,
Rdl.ItemsChoiceType14.Left,
Rdl.ItemsChoiceType14.Top,
Rdl.ItemsChoiceType14.Style,
Rdl.ItemsChoiceType14.CanGrow,
            };
            return textbox;
        }
        private Rdl.StyleType CreateTextboxStyle(int num)
        {
            Rdl.StyleType style = new Rdl.StyleType();
            /* Rdl.ItemsChoiceType5.FontWeight,
 Rdl.ItemsChoiceType5.FontSize,
 Rdl.ItemsChoiceType5.Color,*/
            if (num == 1)
            {
                style.Items = new object[]
                {"Arial Black","700","22pt","Black"
                };
            }
            else if (num == 2)
            {
                style.Items = new object[]
                {"Arial Black","700","10pt","Black"
                };
            }
            else if (num == 3)
            {
                style.Items = new object[]
                {"Arial Black","700","10pt","Black"
                };
            }
            style.ItemsElementName = new Rdl.ItemsChoiceType5[]
            {
                Rdl.ItemsChoiceType5.FontFamily,
Rdl.ItemsChoiceType5.FontWeight,
Rdl.ItemsChoiceType5.FontSize,
Rdl.ItemsChoiceType5.Color,
            };
            return style;
        }

        private Rdl.DataSourcesType CreateDataSources()
        {
            Rdl.DataSourcesType dataSources = new Rdl.DataSourcesType();
            dataSources.DataSource = new Rdl.DataSourceType[] { CreateDataSource() };
            return dataSources;
        }

        private Rdl.DataSourceType CreateDataSource()
        {
            Rdl.DataSourceType dataSource = new Rdl.DataSourceType();
            dataSource.Name = "DummyDataSource";
            dataSource.Items = new object[] { CreateConnectionProperties() };
            return dataSource;
        }

        private Rdl.ConnectionPropertiesType CreateConnectionProperties()
        {
            Rdl.ConnectionPropertiesType connectionProperties = new Rdl.ConnectionPropertiesType();
            connectionProperties.Items = new object[]
                {
                    "",
                    "SQL",
                };
            connectionProperties.ItemsElementName = new Rdl.ItemsChoiceType[]
                {
                    Rdl.ItemsChoiceType.ConnectString,
                    Rdl.ItemsChoiceType.DataProvider,
                };
            return connectionProperties;
        }

        private Rdl.BodyType CreateBody()
        {
            Rdl.BodyType body = new Rdl.BodyType();
            body.Items = new object[]
                {
                    CreateReportItems(),
                    "0.5cm",
                    CreateTablixStyle()
                };
            body.ItemsElementName = new Rdl.ItemsChoiceType30[]
                {
                    Rdl.ItemsChoiceType30.ReportItems,
                    Rdl.ItemsChoiceType30.Height,
                    Rdl.ItemsChoiceType30.Style
                };
            return body;
        }
        private Rdl.StyleType CreateTablixStyle()
        {
            Rdl.StyleType style = new Rdl.StyleType();
            
            style.Items = new object[]
                {"Arial Black","4pt","Black" };
            style.ItemsElementName = new Rdl.ItemsChoiceType5[]
            {
                Rdl.ItemsChoiceType5.FontFamily,

Rdl.ItemsChoiceType5.FontSize,
Rdl.ItemsChoiceType5.Color,
            };
            return style;
        }
        private Rdl.ReportItemsType CreateReportItems()
        {
            Rdl.ReportItemsType reportItems = new Rdl.ReportItemsType();
            TableRdlGenerator tableGen = new TableRdlGenerator();
            tableGen.Fields = m_selectedFields;
            reportItems.Items = new object[] { tableGen.CreateTable() };
            return reportItems;
        }

        private Rdl.DataSetsType CreateDataSets()
        {
            Rdl.DataSetsType dataSets = new Rdl.DataSetsType();
            dataSets.DataSet = new Rdl.DataSetType[] { CreateDataSet() };
            return dataSets;
        }

        private Rdl.DataSetType CreateDataSet()
        {
            Rdl.DataSetType dataSet = new Rdl.DataSetType();
            dataSet.Name = "MyData";
            dataSet.Items = new object[] { CreateQuery(), CreateFields() };
            return dataSet;
        }

        private Rdl.QueryType CreateQuery()
        {
            Rdl.QueryType query = new Rdl.QueryType();
            query.Items = new object[]
                {
                    "DummyDataSource",
                    "",
                };
            query.ItemsElementName = new Rdl.ItemsChoiceType2[]
                {
                    Rdl.ItemsChoiceType2.DataSourceName,
                    Rdl.ItemsChoiceType2.CommandText,
                };
            return query;
        }

        private Rdl.FieldsType CreateFields()
        {
            Rdl.FieldsType fields = new Rdl.FieldsType();

            fields.Field = new Rdl.FieldType[m_allFields.Count];
            for (int i = 0; i < m_allFields.Count; i++)
            {
                fields.Field[i] = CreateField(m_allFields[i]);
            }

            return fields;
        }

        private Rdl.FieldType CreateField(String fieldName)
        {
            Rdl.FieldType field = new Rdl.FieldType();
            field.Name = fieldName;
            field.Items = new object[] { fieldName };
            field.ItemsElementName = new Rdl.ItemsChoiceType1[] { Rdl.ItemsChoiceType1.DataField };
            return field;
        }

        public void WriteXml(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Rdl.Report));
            /* using (StreamWriter writer = new StreamWriter(@"C:\Users\informatic\Documents\Visual Studio 2015\Projects\dynamicTable\bin\Debug\prueba2.txt"))
             {
                 serializer.Serialize(writer, CreateReport());
             }*/

            //
            serializer.Serialize(stream, CreateReport());
        }
    }
}
