using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace DataGridAutoFilter
{

    [ToolboxData("<{0}:editableElphGrid runat=\"server\"></{0}:editableElphGrid>")]
    //ParseChildren allow to work with templates inside the custom datagrid
    [ParseChildren(true)]
    public class editableElphGrid : System.Web.UI.WebControls.DataGrid
    {
        /// <summary>
        /// Points to the method that returns a datatable with data
        /// Apunta a la funcion q devolvera un dataTable con los datos
        /// </summary>
        public delegate DataTable getData();
        /// <summary>
        /// Obtain data event 
        /// Evento de obtener datos
        /// </summary>
        public event getData GetData;
        /// <summary>
        /// Points to method(to update data)
        /// Apunta a un metodo q nos servira para modificar datos
        /// </summary>
        public delegate void updateData(int dataKey, DataGridCommandEventArgs e);
        /// <summary>
        /// Update data event
        /// </summary>
        public event updateData UpdateData;
        /// <summary>
        /// Points to method(delete data)
        /// Apunta al metodo q usaremos para eliminar los datos
        /// </summary>
        public delegate void deleteData(int dataKey, DataGridCommandEventArgs e);
        /// <summary>
        /// delete data event
        /// </summary>
        public event deleteData DeleteData;
        /// <summary>
        /// Cancel data
        /// </summary>
        public delegate void cancelData(int dataKey, DataGridCommandEventArgs e);
        /// <summary>
        /// cancel data event
        /// </summary>
        public event cancelData CancelData;
        /// <summary>
        /// edit data
        /// </summary>
        public delegate void editData(int dataKey, DataGridCommandEventArgs e);
        /// <summary>
        /// edit data event
        /// </summary>
        public event editData EditData;

        //  Note: the params passed on the delegates
        //    dataKey: is the DataKey field of the edited/canceled/updated/deleted row
        //    e: for work with the cells of the grid

        /// <summary>
        /// First sorting field
        /// campo por el q se ordenara en la primera carga
        /// </summary>
        private string _cFirst = "";
        /// <summary>
        /// First sorting field
        /// campo por el q se ordenara en la primera carga
        /// </summary>
        public string FirstSortingField
        {
            get
            { return _cFirst; }
            set
            { _cFirst = value; }
        }
        // Inicializamos el grid
        protected override void OnInit(EventArgs e)
        {
            //paginacion=true
            this.AllowPaging = true;
            //ordenacion=true
            this.AllowSorting = true;
            //evento cambio de pagina
            //ADD the events
            this.PageIndexChanged += new DataGridPageChangedEventHandler(cPage_elphGrid);
            //evento ordenar
            this.SortCommand += new DataGridSortCommandEventHandler(Order_elphGrid);
            //evento de cargar datos
            this.Load += new EventHandler(elphGrid_Load);
            this.EditCommand += new DataGridCommandEventHandler(Edit_elphGrid);
            this.CancelCommand += new DataGridCommandEventHandler(Cancel_elphGrid);
            this.DeleteCommand += new DataGridCommandEventHandler(Delete_elphGrid);
            this.UpdateCommand += new DataGridCommandEventHandler(Update_elphGrid);
            //clear the columns
            //limpiamos las columnas
            this.Columns.Clear();
            //create the editComandColumn an deleteColumn
            //creamos las columnas de editar i eliminar
            EditCommandColumn col = new EditCommandColumn();
            col.ButtonType = ButtonColumnType.LinkButton;
            col.CancelText = "Cancel";
            col.EditText = "Edit";
            col.UpdateText = "Update";
            this.Columns.Add(col);

            ButtonColumn delCol = new ButtonColumn();
            delCol.CommandName = "Delete";
            delCol.ButtonType = ButtonColumnType.LinkButton;
            delCol.Text = "Delete";
            this.Columns.Add(delCol);
        }
        private void elphGrid_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                //changed Session object for Viewstate
                if (this.AllowSorting && this._cFirst != "")
                    this.ViewState.Add("_orderBy", this._cFirst);
                this.ViewState.Add("_orderType", "ASC");
                this.DataSource = CreateDataSet();
                this.DataBind();
            }
        }
        private void cPage_elphGrid(object sender, DataGridPageChangedEventArgs e)
        {
            //PAging
            this.CurrentPageIndex = e.NewPageIndex;
            this.DataSource = CreateDataSet();
            this.DataBind();
        }
        private ICollection CreateDataSet()
        {
            //this.ObtenerDatos call a external function that return data
            DataTable dt = this.GetData();
            if (this.AllowSorting && this.ViewState["_orderBy"] != null)
            {
                //sort the grid
                if (this.ViewState["_orderType"].ToString() == "ASC")
                    dt.DefaultView.Sort = (string)this.ViewState["_orderBy"].ToString() + " ASC";
                else if (this.ViewState["_orderType"].ToString() == "DESC")
                    dt.DefaultView.Sort = (string)this.ViewState["_orderBy"] + " DESC";
            }
            return dt.DefaultView;
        }
        public void Order_elphGrid(object sender, DataGridSortCommandEventArgs e)
        {
            this.ViewState["_orderBy"] = (string)e.SortExpression;
            if (this.ViewState["_orderType"].ToString() == "ASC")
                this.ViewState["_orderType"] = "DESC";
            else if (this.ViewState["_orderType"].ToString() == "DESC")
                this.ViewState["_orderType"] = "ASC";
            this.DataSource = CreateDataSet();
            this.DataBind();
        }
        public void Edit_elphGrid(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(this.DataKeys[e.Item.ItemIndex]);
            this.EditData(id, e);
            this.EditItemIndex = e.Item.ItemIndex;
            this.DataSource = CreateDataSet();
            this.DataBind();
        }
        public void Delete_elphGrid(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(this.DataKeys[e.Item.ItemIndex]);
            this.DeleteData(id, e);
            this.EditItemIndex = -1;
            this.DataSource = CreateDataSet();
            this.DataBind();
        }
        public void Update_elphGrid(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(this.DataKeys[e.Item.ItemIndex]);
            this.UpdateData(id, e);
            this.EditItemIndex = -1;
            this.DataSource = CreateDataSet();
            this.DataBind();
        }
        public void Cancel_elphGrid(object sender, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int id = Convert.ToInt32(this.DataKeys[e.Item.ItemIndex]);
            this.CancelData(id, e);
            this.EditItemIndex = -1;
            this.DataSource = CreateDataSet();
            this.DataBind();
        }
    }
}
