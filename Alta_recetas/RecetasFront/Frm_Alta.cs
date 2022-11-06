
using Newtonsoft.Json;
using RecetasBack.Client;
using RecetasBack.negocio;
using RecetasSLN.dominio;
using System.Text;

namespace RecetasFront
{

    public enum Accion
    {
        CREATE,
        UPDATE
    }

    public partial class Frm_Alta : Form
    {
        private Receta nueva;
        private Aplicacion app;

        public Frm_Alta()
        {
            InitializeComponent();
            nueva = new Receta();
            app = new Aplicacion();
            ConsultarUltimaReceta();
            ActualizarTXT();
        }
        private async Task ConsultarUltimaReceta()
        {
            string url = "https://localhost:7142/api/Recetas/proximoID";
            var data = await ClienteSingleton.GetInstance().GetAsync(url);
            var v = JsonConvert.DeserializeObject(data);

            lblNro.Text = "Receta #: " + v;

        }
        private async Task CargarCombo()
        {

            string url = "https://localhost:7142/api/Recetas/ingrediente";
            var data = await ClienteSingleton.GetInstance().GetAsync(url);
            List<Ingrediente> lst = JsonConvert.DeserializeObject<List<Ingrediente>>(data);

            cboProducto.DataSource = lst;
            cboProducto.DisplayMember = "Nombre";
            cboProducto.ValueMember = "IngredienteID";

        }
        private async Task<bool> GrabarRecetaAsync(string data)
        {
            string url = "https://localhost:7142/api/Recetas/GuardarReceta";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                string response = await result.Content.ReadAsStringAsync();
                return response.Equals("OK");

            }
        }
        private async void btnAceptar_Click(object sender, EventArgs e)
        {

            if (txtCheff.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar un Chef", "Controls", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCheff.Focus();
                return;
            }

            if (dgvDetalles.Rows.Count < 3)
            {
                MessageBox.Show("Debe ingresar 3 ingredientes como mínimo", "Controls", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboProducto.Focus();
                return;

            }
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar un Nombre", "Controls", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Focus();
                return;
            }


            nueva.RecetaNro = app.ProximaReceta();
            nueva.Nombre = txtNombre.Text;
            nueva.Chef = txtCheff.Text;
            nueva.TipoReceta = Convert.ToInt32(cboTipo.SelectedIndex);



            string data = JsonConvert.SerializeObject(nueva);
            var result = await GrabarRecetaAsync(data);
            string final = result.ToString();

            if (final != null)
            {
                MessageBox.Show("Receta guardada con éxito!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Error al intentar grabar la Receta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
            }




        }
        public void ActualizarTXT()
        {
            txtNombre.Text = "Ingrese nombre de su receta";
            txtCheff.Text = "Ingrese el nombre del cheff";
            cboTipo.SelectedIndex = 0;
        }
        private async void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            await CargarCombo();

        }
        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboTipo.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar una opcion de receta", "Controls", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboProducto.Text.Equals(string.Empty))
            {
                MessageBox.Show("Debe seleccionar un ingrediente", "Controls", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(nudCantidad.Text) || !int.TryParse(nudCantidad.Text, out _))
            {
                MessageBox.Show("Debe ingresar una cantidad válida", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["Ingrediente"].Value.ToString().Equals(cboProducto.Text))
                {
                    MessageBox.Show("Este ingrediente ya está cargado.", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            DetalleReceta item = new DetalleReceta();

            Ingrediente oIngrediente = (Ingrediente)cboProducto.SelectedItem;
            item.Ingrediente = oIngrediente;
            item.Cantidad = (int)nudCantidad.Value;

            nueva.AgregarReceta(item);

            dgvDetalles.Rows.Add(new string[] { "", oIngrediente.Nombre, item.Cantidad.ToString() });

            ActualizarTotales();
        }
        private async void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtNombre.Focus();
            txtCheff.Text = string.Empty;
            cboTipo.Text = string.Empty;
            dgvDetalles.Rows.Clear();
            lblTotalIngredientes.Text = "Total de ingredientes:";
            await ConsultarUltimaReceta();
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                return;
            }
        }
        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 3)
            {
                nueva.Eliminar(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
            }
        }
        private void ActualizarTotales()
        {
            lblTotalIngredientes.Text = "Total de ingredientes:" + dgvDetalles.Rows.Count;
        }


    }
}
