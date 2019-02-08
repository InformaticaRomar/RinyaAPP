using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Collections;

namespace DataGridAutoFilter
{
    /// </summary>
    [ParseChildren(true)]
    [PersistChildren(true)]
    [
        ToolboxData("<{0}:DataGridAutoFilter runat=server></{0}:DataGridAutoFilter>")
    ]
   
    public class DataGridAutoFilter : System.Web.UI.WebControls.DataGrid, IPostBackEventHandler
    {
       // public ArrayList list;
        public ArrayList sort;
        public bool filter;


        /// <summary>
        /// Gets or sets a value that indicates whether the auto filter is displayed in the DataGridAF.DataGridAutoFilter control.
        /// </summary>
        [
            Bindable(true),
            Category("Appearance"),
            Description("Whether to show the control's auto filter."),
            DefaultValue(true),
        ]
        public bool ShowFilter
        {
            get { return filter; }
            set { filter = value; }
        }


        /// <summary>
        /// Override the DataGrid constructor.
        /// </summary>
        public DataGridAutoFilter() : base()
        {
            // create the ArrayList to contain the HtmlSelect controls and the SortedList objects added to the header items;
            list = new ArrayList();
            sort = new ArrayList();
            filter = true;
        }
        public ArrayList list
        {
            get
            {
                if (ViewState["list"] != null)
                    return (ArrayList)ViewState["list"];
                return null;
            }

            set
            {
                ViewState["list"] = value;
            }
        }

        /// <summary>
        /// Override the OnItemDataBound event.
        /// </summary>
        /// <param name="e"></param>
        override protected void OnItemDataBound(DataGridItemEventArgs e)
        {
        
            switch (e.Item.ItemType)
            {
                case ListItemType.Header:
                    if (filter == false) break;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        // column header text;
                        Label headerText = new Label();
                        headerText.Text = e.Item.Cells[i].Text;
                        e.Item.Cells[i].Controls.Add(headerText);

                        // add a new line;
                        e.Item.Cells[i].Controls.Add(new HtmlGenericControl("br"));
                        ClientScriptManager cs = Page.ClientScript;
                        // add a select element, with "onchange" handler that causes a postback to RaisePostBackEvent;
                        HtmlSelect select = new HtmlSelect();
                        //string()
                        select.Attributes.Add("AutoPostBack", "True");
                        select.Attributes.Add("onchange", Page.ClientScript.GetPostBackEventReference(this, select.Value.ToString() ));
                        e.Item.Cells[i].Controls.Add(select);
                        // load the select element into the ArrayList;
                        list.Add(select);

                        // add a SortedList object, with an empty item;
                        SortedList sorted = new SortedList();
                        sorted.Add("", "");
                        // load the SortedList object into the ArrayList;
                        sort.Add(sorted);
                    }
                    break;

                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                    if (filter == false) break;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        // fill the SortedList object with the "distinct" values from each columns;
                        SortedList sorted = (SortedList)sort[i];
                        if (sorted.ContainsValue(e.Item.Cells[i].Text) == false)
                            sorted.Add(e.Item.Cells[i].Text, e.Item.Cells[i].Text);
                    }
                    break;

                case ListItemType.Footer:
                    if (filter == false) break;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        SortedList sorted = (SortedList)sort[i];
                        HtmlSelect select = (HtmlSelect)list[i];
                        // load sorted item into select element;
                        for (int j = 0; j < sorted.Count; j++)
                            select.Items.Add(sorted.GetByIndex(j).ToString());
                        sorted.Clear();
                    }
                    sort.Clear();
                    break;
            }

            base.OnItemDataBound(e);
        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //code here
        }
        //t/ <summary>
        /// Implement the method that gets called by the ASP.NET framework when our control causes a postback.
        /// </summary>
        /// <param name="eventArgument"></param>
        public void RaisePostBackEvent( string eventArgument)
           {

            ///DataGridItemEventArgs args2 = new DataGridItemEventArgs(this.Items[0]);
            //OnItemDataBound(args2);
            // this.DataBind();
            // this.Page.Controls
          //  this.DataBind();
                   if (filter == false) return;
               ArrayList lista = new ArrayList();

               foreach (DataGridItem a in Items)
               {
                  // Array b = new Array();
                   //a.Cells.Count
                   lista.Add(a.Cells);
               }

               // for each row;
               for (int i = 0; i < Items.Count; i++)
               {
                   // for each column;
                   for (int j = 0; j < Items[i].Cells.Count; j++)
                   {
                       HtmlSelect select = null;//(HtmlSelect)list[j];
                     //  HtmlSelect select2 = Items[i].Cells[j];//(HtmlSelect)
                    //   if (select.SelectedIndex > 0)
                    if(false)
                       {
                           // hide rows with a not selected value;
                           if (Items[i].Cells[j].Text != select.Items[select.SelectedIndex].Text)
                               Items[i].Visible = false;
                       }
                   }
               }
           }
    }

}
