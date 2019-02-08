using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;

namespace rinya_app
{
    public partial class Formulario_web1 : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["datos"] == null)
            {
                DefinirColumnasLote_interno("039222");

            }
            else { DefinirColumnasLote_interno("039222"); }
        }
        private void DataBindGrid()
        {
            GridView1.DataSource = Session["datos"];
            GridView1.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["datos"] = Rellenar_Grid("039222"); 
                DataBindGrid();
            }
        }

        private void DefinirColumnasLote_interno(string loteInterno)
        {
            string sql = "SELECT DISTINCT Organolectico_Carac.Cod_Caracteristica,Organolectico_Carac.Caracteristica FROM CARACTERISTICAS_ARTICULO inner join Organolectico_Carac ON CARACTERISTICAS_ARTICULO.Caracteristica = Organolectico_Carac.Cod_Caracteristica where CARACTERISTICAS_ARTICULO.Articulo in (select distinct Artículo from [STOCK PARTIDAS] where loteinterno='" + loteInterno + "')";
            Quality con = new Quality();
            DataTable Columnas = con.Sql_Datatable(sql);
            GridView1.DataKeyNames = new string[] { "Matricula" };

            GridView1.Columns.Clear();
            TemplateField tempArticulo = new TemplateField();
            tempArticulo.HeaderTemplate = new GridViewHeaderTemplate("Articulo");
            tempArticulo.ItemTemplate = new GridViewItemTemplate("Articulo");
            //tempArticulo.EditItemTemplate = new GridViewEditTemplate("Articulo");
            GridView1.Columns.Add(tempArticulo);

            TemplateField tempDescripcion = new TemplateField();
            tempDescripcion.HeaderTemplate = new GridViewHeaderTemplate("Descripcion");
            tempDescripcion.ItemTemplate = new GridViewItemTemplate("Descripcion");
           // tempDescripcion.EditItemTemplate = new GridViewEditTemplate("Descripcion");
            GridView1.Columns.Add(tempDescripcion);

            TemplateField tempFechaCaducidad = new TemplateField();
            tempFechaCaducidad.HeaderTemplate = new GridViewHeaderTemplate("FechaCaducidad");
            tempFechaCaducidad.ItemTemplate = new GridViewItemTemplate("FechaCaducidad");
            //tempFechaCaducidad.EditItemTemplate = new GridViewEditTemplate("FechaCaducidad");
            GridView1.Columns.Add(tempFechaCaducidad);

            TemplateField tempUD_ACTUAL = new TemplateField();
            tempUD_ACTUAL.HeaderTemplate = new GridViewHeaderTemplate("UD ACTUAL");
            tempUD_ACTUAL.ItemTemplate = new GridViewItemTemplate("UD_ACTUAL");
            //tempUD_ACTUAL.EditItemTemplate = new GridViewEditTemplate("UD_ACTUAL");
            GridView1.Columns.Add(tempUD_ACTUAL);

            TemplateField tempKG_ACTUAL = new TemplateField();
            tempKG_ACTUAL.HeaderTemplate = new GridViewHeaderTemplate("KG ACTUAL");
            tempKG_ACTUAL.ItemTemplate = new GridViewItemTemplate("KG_ACTUAL");
            //tempKG_ACTUAL.EditItemTemplate = new GridViewEditTemplate("KG_ACTUAL");
            GridView1.Columns.Add(tempKG_ACTUAL);

            TemplateField tempESTADO = new TemplateField();
            tempESTADO.HeaderTemplate = new GridViewHeaderTemplate("ESTADO");
            tempESTADO.ItemTemplate = new GridViewItemTemplate("ESTADO");
            //tempESTADO.EditItemTemplate = new GridViewEditTemplate("ESTADO");
            GridView1.Columns.Add(tempESTADO);

            TemplateField tempMatricula = new TemplateField();
            tempMatricula.HeaderTemplate = new GridViewHeaderTemplate("Matricula");
            tempMatricula.ItemTemplate = new GridViewItemTemplate("Matricula");
            //tempMatricula.EditItemTemplate = new GridViewEditTemplate("Matricula");
            GridView1.Columns.Add(tempMatricula);

            TemplateField tempFECHACREACION = new TemplateField();
            tempFECHACREACION.HeaderTemplate = new GridViewHeaderTemplate("FECHACREACION");
            tempFECHACREACION.ItemTemplate = new GridViewItemTemplate("FECHACREACION");
           // tempFECHACREACION.EditItemTemplate = new GridViewEditTemplate("FECHACREACION");
            GridView1.Columns.Add(tempFECHACREACION);

            TemplateField tempHORA = new TemplateField();
            tempHORA.HeaderTemplate = new GridViewHeaderTemplate("HORA");
            tempHORA.ItemTemplate = new GridViewItemTemplate("HORA");
            //tempHORA.EditItemTemplate = new GridViewEditTemplate("HORA");
            GridView1.Columns.Add(tempHORA);


            foreach (DataRow row in Columnas.AsEnumerable())
            {

                TemplateField tempfield = new TemplateField();
                tempfield.HeaderTemplate = new GridViewHeaderTemplate(row[1].ToString());
                tempfield.ItemTemplate = new GridViewItemTemplate(row[1].ToString());
                tempfield.EditItemTemplate = new GridViewEditTemplate(row[1].ToString());
                GridView1.Columns.Add(tempfield);
            }

        }
        private DataTable Rellenar_Grid(string loteInterno)
        {
            DataTable dt = new DataTable();
            DefinirColumnasLote_interno(loteInterno);
            Quality con = new Quality();
            dt = con.Sql_Procedure_Datatable("OBTENER_DATOS_ORGANOLEPTICO", new string[,] { { "LOTEINTERNO", loteInterno } });

            return dt;

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            DataBindGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            DataBindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string Id = GridView1.DataKeys[e.RowIndex].Value.ToString();

            GridViewRow row = GridView1.Rows[e.RowIndex];
           Label lblArticulo = row.FindControl("lblArticulo") as Label;
            string articulo = lblArticulo.Text;
            for (int i =8; i < GridView1.Columns.Count; i++)
            {
                var prueba = row.DataItem;
             //   TextBox txtDescripcion = (TextBox) row[8];
            }
            GridView1.EditIndex = -1;
            DataBindGrid();
        }

        }

    public class GridViewHeaderTemplate : ITemplate
    {
        string text;

        public GridViewHeaderTemplate(string text)
        {
            this.text = text;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            Literal lc = new Literal();
            lc.Text = text;
          //  lc.ID = text;
            container.Controls.Add(lc);

        }
    }

    public class GridViewEditTemplate : ITemplate
    {
        private string columnName;

        public GridViewEditTemplate(string columnName)
        {
            this.columnName = columnName;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            TextBox tb = new TextBox();
            tb.ID = string.Format("txt{0}", columnName);
            tb.EnableViewState = false;
            tb.DataBinding += new EventHandler(tb_DataBinding);

            container.Controls.Add(tb);
        }

        void tb_DataBinding(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;

            GridViewRow row = (GridViewRow)t.NamingContainer;

            string RawValue = DataBinder.Eval(row.DataItem, columnName).ToString();

            t.Text = RawValue;
        }
    }

    public class GridViewItemTemplate : ITemplate
    {
        private string columnName;

        public GridViewItemTemplate(string columnName)
        {
            this.columnName = columnName;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            Literal lc = new Literal();
            Label lb = new Label();
            lb.ID = string.Format("lbl{0}", columnName);
            lb.DataBinding += new EventHandler(lc_DataBinding);
            //lc.DataBinding += new EventHandler(lc_DataBinding);
            
            container.Controls.Add(lb);

           

        }

        void lc_DataBinding(object sender, EventArgs e)
        {
            Label l = (Label)sender;

            GridViewRow row = (GridViewRow)l.NamingContainer;
           

            string RawValue = DataBinder.Eval(row.DataItem, columnName).ToString();

            l.Text = RawValue;
        }
    }


    public class GridViewItemCheckTemplate : ITemplate
    {
        private string columnName;

        public GridViewItemCheckTemplate(string columnName)
        {
            this.columnName = columnName;
            this.CanEdit = false;
        }

        public bool CanEdit { get; set; }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            CheckBox check = new CheckBox();
            check.ID = string.Format("chk{0}", columnName);
            check.Enabled = this.CanEdit;
            check.DataBinding += new EventHandler(check_DataBinding);

            container.Controls.Add(check);

        }

        void check_DataBinding(object sender, EventArgs e)
        {
            CheckBox check = (CheckBox)sender;

            GridViewRow row = (GridViewRow)check.NamingContainer;

            string value = DataBinder.Eval(row.DataItem, columnName).ToString();

            check.Checked = bool.Parse(value);
        }

    }
}