namespace RecetasApi.Resultados
{
    public class Resultado
    {

        public bool Ok { get; set; } = true;
        public string Error { get; set; }

        public int StatusCode { get; set; }
        public void SetError(string mensajeError)
        {
            Ok = false;
            Error = mensajeError;
        }


    }
}
