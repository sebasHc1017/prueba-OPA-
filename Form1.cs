using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace planificadorDeEscalada
{


    public partial class Form1 : Form {

        private TextBox pesoMaximo;
        private TextBox caloriasMinimas;
        private DataGridView dataGridView;
        private Button agregarFilaButton;
        private Button limpiarTablaButton;
        private Button calcular;
        private Label labelPesoMaximo;
        private Label labelCaloriasMinimas;

        public Form1() {

            InitializeComponent();

            pesoMaximo = new TextBox();
            caloriasMinimas = new TextBox();
            pesoMaximo.Location = new System.Drawing.Point(200, 50);
            pesoMaximo.Size = new System.Drawing.Size(400, 50);
            pesoMaximo.KeyPress += TextBox_KeyPress; // Agregar el evento KeyPress para validar que solo ingrese numero 


            caloriasMinimas.Location = new System.Drawing.Point(200, 80);
            caloriasMinimas.Size = new System.Drawing.Size(400, 50);
            caloriasMinimas.KeyPress += TextBox_KeyPress; // Agregar el evento KeyPress para validar que solo ingrese numero 

            labelPesoMaximo = new Label();
            labelPesoMaximo.Text = "Peso Máximo:";
            labelPesoMaximo.Location = new System.Drawing.Point(100, 50);

            labelCaloriasMinimas = new Label();
            labelCaloriasMinimas.Text = "Calorías Mínimas:";
            labelCaloriasMinimas.Location = new System.Drawing.Point(100, 80);
            // boton para agregar mas filas a la tabla 
            agregarFilaButton = new Button();
            agregarFilaButton.Text = "Agregar Fila";
            agregarFilaButton.Location = new Point(320, 120);
            agregarFilaButton.Click += AgregarFilaButton_Click;

            // boton para limpiar las filas a la tabla 
            limpiarTablaButton = new Button();
            limpiarTablaButton.Text = "Limpiar Tabla";
            limpiarTablaButton.Location = new Point(400, 120);
            limpiarTablaButton.Click += LimpiarTablaButton_Click;

            // button para realizar el calculo  
            calcular = new Button();
            calcular.Text = "Calcular";
            calcular.Location = new Point(360, 350);
            calcular.Click += calculo;


            InitializeComponentCustom();
        }
        private void InitializeComponentCustom()
        {
            dataGridView = new DataGridView();

            // Configuracion del DataGridView
            dataGridView.Location = new System.Drawing.Point(270, 150);
            dataGridView.Size = new System.Drawing.Size(270, 180);
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "peso";
            dataGridView.Columns[1].Name = "calorias";
            dataGridView.KeyPress += TextBox_KeyPress;

            dataGridView.AllowUserToAddRows = false;
            this.Controls.Add(limpiarTablaButton);
            this.Controls.Add(agregarFilaButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Controls.Add(labelPesoMaximo);
            this.Controls.Add(labelCaloriasMinimas);
            this.Controls.Add(pesoMaximo);
            this.Controls.Add(caloriasMinimas);
            this.Controls.Add(dataGridView);
            this.Controls.Add(calcular);
        }

        // Evento KeyPress para validar que  solo ingrese  números
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void AgregarFilaButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Add("", "");
        }
        private void LimpiarTablaButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void calculo(object sender, EventArgs e)
        {
            GuardarDatosTabla();
        }


        private void GuardarDatosTabla()
        {
            double totalPeso = 0;
            double totalCalorias = 0;

            List<string> listaPesos = new List<string>();
            List<string> listaCalorias = new List<string>();
            List<string> listaResultados = new List<string>();

            int indiceFila = 1;

            foreach (DataGridViewRow fila in dataGridView.Rows)
            {
                if (!fila.IsNewRow)
                {
                    string pesoTexto = fila.Cells["peso"].Value?.ToString() ?? string.Empty;
                    string caloriasTexto = fila.Cells["calorias"].Value?.ToString() ?? string.Empty;

                    if (double.TryParse(pesoTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double peso) &&
                        double.TryParse(caloriasTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double calorias))
                    {
                        totalPeso += peso;
                        totalCalorias += calorias;

                        listaPesos.Add(pesoTexto);
                        listaCalorias.Add(caloriasTexto);

                        string resultadoFila = $"E{indiceFila} - Peso: {peso}, Calorías: {calorias}";
                        listaResultados.Add(resultadoFila);

                        indiceFila++;
                    }
                }
            }

            string pesoMaximoTexto = pesoMaximo.Text;
            string caloriasMinimasTexto = caloriasMinimas.Text;

            string mensajeInicio = $"Peso Máximo: {pesoMaximoTexto}, Calorías Mínimas: {caloriasMinimasTexto}";

            string resultado = $"{mensajeInicio}\n\nResultados:\n{string.Join("\n", listaResultados)}\n\nTotal Peso: {totalPeso}\nTotal Calorías: {totalCalorias}";

            if (double.TryParse(pesoMaximoTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double pesoMaximoValor) &&
                totalPeso > pesoMaximoValor)
            {
                resultado += $"\n\n¡Alerta! El peso total es mayor que el peso máximo ingresado.";
            }

            if (double.TryParse(caloriasMinimasTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double caloriasMinimasValor) &&
                totalCalorias < caloriasMinimasValor)
            {
                resultado += $"\n\n¡Alerta! Las calorías totales son menores que las calorías mínimas ingresadas.";
            }

            MessageBox.Show(resultado, "Datos Guardados");
        }


    }

}
