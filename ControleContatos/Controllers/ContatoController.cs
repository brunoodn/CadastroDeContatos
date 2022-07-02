using ControleContatos.Filters;
using ControleContatos.Models;
using ControleContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ControleContatos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        public ContatoController(IContatoRepositorio contatoRepositorio)
        {
            _contatoRepositorio = contatoRepositorio;
        }
        public IActionResult Index()
        {
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodos();
            return View(contatos);
        }
        public IActionResult Criar()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }
        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }
        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _contatoRepositorio.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Contato deletado com sucesso.";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, nao conseguimos deletado com sucesso.";
                }
                return RedirectToAction("Index");
            }
            catch (System.Exception erro)
            {

                TempData["MensagemErro"] = $"Ops, Erro ao deletar o contato. {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso.";
                    return RedirectToAction("Index");
                }

                return View(contato);
            }
            catch (System.Exception erro)
            {

                _contatoRepositorio.Adicionar(contato);
                TempData["MensagemErro"] = $"Ops, Não conseguimos cadastrar seu contato. Detalhe do erro: {erro.Message}" ;
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public IActionResult Editar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _contatoRepositorio.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato alterado com sucesso.";
                    return RedirectToAction("Index");
                }
                return View("Editar", contato);
            }
            catch (System.Exception erro)
            {

                _contatoRepositorio.Atualizar(contato);
                TempData["mensagemErro"] = $"Ops, Erro ao atualizar o contato. Detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
            
        }
    }
}
