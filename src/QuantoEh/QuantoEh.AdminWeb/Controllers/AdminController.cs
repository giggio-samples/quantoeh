using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics.Management;
using Microsoft.WindowsAzure.ServiceRuntime;
using QuantoEh.Dominio;
using QuantoEh.Infra;

namespace QuantoEh.AdminWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITweetsParaProcessar _tweetsParaProcessar;
        private readonly AvaliadorDeInterrupcao _avaliadorDeInterrupcao;

        public AdminController()
            : this(new TweetsParaProcessar(), new AvaliadorDeInterrupcao())
        {

        }

        public AdminController(ITweetsParaProcessar tweetsParaProcessar, AvaliadorDeInterrupcao avaliadorDeInterrupcao)
        {
            _tweetsParaProcessar = tweetsParaProcessar;
            _avaliadorDeInterrupcao = avaliadorDeInterrupcao;
        }

        public ActionResult SetarLastId(ulong id)
        {
            _tweetsParaProcessar.ArmazenarUltimoIdPesquisado(id);
            return View("UltimoIdConfigurado", id);
        }

        public ActionResult Interromper(bool? id)
        {
            var interrompido = id == null || id.Value;
            _avaliadorDeInterrupcao.SetaInterrupcao(interrompido);
            return View("Interrompido", interrompido);
        }

        public ActionResult TransferirDiagnostico()
        {
            var storageAccount = ConfiguracaoArmazenamentoAzure.ObterContaDeArmazenamento();
            var deploymentDiagnosticManager = new DeploymentDiagnosticManager(storageAccount, RoleEnvironment.DeploymentId);
            var roleInstanceDiagnosticManager = deploymentDiagnosticManager.GetRoleInstanceDiagnosticManager(RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);
            var requestID = roleInstanceDiagnosticManager.BeginOnDemandTransfer(
                DataBufferName.Logs,
                new OnDemandTransferOptions
                {
                    From = DateTime.UtcNow.Subtract(new TimeSpan(3, 0, 0)),
                    To = DateTime.UtcNow,
                    LogLevelFilter = LogLevel.Verbose
                });
            return View("DiagnosticoTransferido", requestID);
        }        
        
        public ActionResult CancelarTransferenciaDeDiagnostico(Guid id)
        {
            var storageAccount = ConfiguracaoArmazenamentoAzure.ObterContaDeArmazenamento();
            var deploymentDiagnosticManager = new DeploymentDiagnosticManager(storageAccount, RoleEnvironment.DeploymentId);
            var roleInstanceDiagnosticManager = deploymentDiagnosticManager.GetRoleInstanceDiagnosticManager(RoleEnvironment.CurrentRoleInstance.Role.Name, RoleEnvironment.CurrentRoleInstance.Id);
            var cancelou = roleInstanceDiagnosticManager.EndOnDemandTransfer(id);
            ViewBag.Id = id;
            ViewBag.Cancelou = cancelou;
            return View("TransferenciaDeDiagnosticoCancelada");
        }


    }
}
