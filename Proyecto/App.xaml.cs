using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.Dialogos;
using Proyecto.MVVM;
using Proyecto.UC;
using System.Windows;

namespace Proyecto
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AndresProyecto2Context _contexto;
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// Constructor de la clase App
        /// </summary>
        public App()
        {
            // Configurar el contenedor de inyección de dependencias
            var serviceCollection = new ServiceCollection();
            // Configurar los servicios
            ConfigureServices(serviceCollection);
            // Construir el proveedor de servicios
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _contexto = new AndresProyecto2Context();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Configurar el contexto de la base de datos
            services.AddDbContext<AndresProyecto2Context>();
            // Configurar el servicio de logging
            services.AddLogging(configure => configure.AddConsole());
            // Registrar repositorios genéricos
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Registrar servicios y vistas aquí
            // En primer lugar registramos la ventana principal
            services.AddSingleton<MainWindow>();
            services.AddTransient<Login>();
            //services.AddTransient<AgregarUsuario>();
            //services.AddTransient<Window1>();
            // A continuación, registramos los repositorios específicos
            // Lo hacemos con AddScoped para que se cree una nueva instancia
            // de cada repositorio por cada petición
            // Esto es útil para evitar problemas de concurrencia
            // Además, registramos cada interfaz de repositorio específico para cada entidad
            services.AddScoped<IGenericRepository<RolHasPermiso>, RolHasPermisoRepository>();
            services.AddScoped<IGenericRepository<Permiso>, PermisoRepository>();
            services.AddScoped<IGenericRepository<Rol>, RolRepository>();  
            services.AddScoped<IGenericRepository<Gasto>, GastoRepository>();
            services.AddScoped<IGenericRepository<Factura>, FacturaRepository>();
            services.AddScoped<IGenericRepository<Trabajadore>, TrabajadorRepository>();
            services.AddScoped<IGenericRepository<Proyecto.Backend.Modelo.Proyecto>, ProyectoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
            services.AddScoped<IGenericRepository<Maquina>, MaquinaRepository>();
            services.AddScoped<IGenericRepository<Modelo>, ModeloRepository>();
            services.AddScoped<IGenericRepository<Estado>, EstadoRepository>();
            services.AddScoped<IGenericRepository<Nomina>, NominaRepository>();

            // Repositorios específicos para cada entidad, si es necesario
            services.AddScoped<RolHasPermisoRepository>();
            services.AddScoped<PermisoRepository>();
            services.AddScoped<RolRepository>();
            services.AddScoped<GastoRepository>();
            services.AddScoped<FacturaRepository>();
            services.AddScoped<TrabajadorRepository>();
            services.AddScoped<ProyectoRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<MaquinaRepository>();
            services.AddScoped<ModeloRepository>();
            services.AddScoped<EstadoRepository>();
            services.AddScoped<NominaRepository>();

            // Registramos las interfaces de usuario
            services.AddTransient<AgregarUsuario>();
            services.AddTransient<AgregarProyecto>();
            services.AddTransient<AgregarMaquina>();
            services.AddTransient<UCVerGastos>();
            services.AddTransient<UCVerIngresos>();
            services.AddTransient<UCListadoUsuarios>();
           

            // ViewModels
            services.AddTransient<MVTrabajador>();
            services.AddTransient<MVGasto>();  
            services.AddTransient<MVFactura>();
            services.AddTransient<MVProyecto>();
            services.AddTransient<MVMaquina>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Se genera la ventana de Login
            var loginWindow = _serviceProvider.GetService<Login>();
            loginWindow.Show();
            base.OnStartup(e);
        }
    }

}
