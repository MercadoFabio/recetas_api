using Newtonsoft.Json;
using RecetasBack.Client;
using RecetasBack.negocio;
using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RecetasFront.Frm_Alta;

namespace RecetasFront
{
    public partial class FrmGet : Form
    {
        private IAplicacion app;
        private Receta oReceta;

        public FrmGet()
        {
            InitializeComponent();
            app = new Aplicacion();

        }
        public async Task cargarRecetas()
        {
            string url = "https://localhost:7142/api/Recetas/receta";
            var data = await ClienteSingleton.GetInstance().GetAsync(url);
            List<Receta> lst = JsonConvert.DeserializeObject<List<Receta>>(data);

            dgvRecetas.DataSource = lst;
        }
        public async Task<bool> EliminarReceta(int id)
        {
            string url = "https://localhost:7142/api/Recetas/eliminarReceta?id=" + id;
            var result = await ClienteSingleton.GetInstance().DeleteAsync(url);
            return result.Equals(true);
        }
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvRecetas.CurrentRow;
            if (row != null)
            {
                int id = Int32.Parse(row.Cells["RecetaNro"].Value.ToString());
                if (MessageBox.Show("¿Esta seguro que desea eliminar la atencion seleccionada?", "Atención!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // int data = Int32.Parse(JsonConvert.SerializeObject(id));
                    //bool respuesta = app.EliminarReceta(id);

                    if (app.EliminarReceta(id))
                    {
                        MessageBox.Show("Atencion eliminada!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await cargarRecetas();
                    }
                    else
                        MessageBox.Show("Error al intentar eliminar la atencion!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        private async void FrmGet_Load(object sender, EventArgs e)
        {
            await cargarRecetas();
            
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea salir?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                return;
            }
        }

       
    }
}


