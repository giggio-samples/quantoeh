using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuantoEh.Dominio;
using QuantoEh.Infra;

namespace QuantoEh.AdminWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITweetsParaProcessar _tweetsParaProcessar;
        private readonly AvaliadorDeInterrupcao _avaliadorDeInterrupcao;

        public AdminController() : this(new TweetsParaProcessar(), new AvaliadorDeInterrupcao())
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

        public ActionResult Interromper(bool ?id)
        {
            var interrompido = id == null || id.Value;
            _avaliadorDeInterrupcao.SetaInterrupcao(interrompido);
            return View("Interrompido", interrompido);
        }
    }
}
