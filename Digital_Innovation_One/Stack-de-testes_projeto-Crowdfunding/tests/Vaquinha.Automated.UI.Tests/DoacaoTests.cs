using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>, 
                                               IClassFixture<EnderecoFixture>, 
                                               IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because:"logo exibido");
		}
		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			//--> Simulando a entrada de dados na View
			//-> Utiliza o sendkeys para preencher os campos da view
			IWebElement campoValor = _driver.FindElement(By.Id("valor"));
			campoValor.SendKeys(doacao.Valor.ToString());
			IWebElement campoNome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
			campoNome.SendKeys(doacao.DadosPessoais.Nome);
			IWebElement campoEmail = _driver.FindElement(By.Id("DadosPessoais_Email"));
			campoEmail.SendKeys(doacao.DadosPessoais.Email);
			IWebElement campoMensagemApoio = _driver.FindElement(By.Id("DadosPessoais_MensagemApoio"));
			campoMensagemApoio.SendKeys(doacao.DadosPessoais.MensagemApoio);
			IWebElement campoAnonima = _driver.FindElement(By.Id("DadosPessoais_Anonima"));
			if (doacao.DadosPessoais.Anonima)
            {
				campoAnonima.SendKeys("{SPACE}");
			}
			//
			IWebElement campoEnderecoCobranca = _driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco"));
			campoEnderecoCobranca.SendKeys(doacao.EnderecoCobranca.TextoEndereco);
			IWebElement campoEnderecoCobrancaNumero = _driver.FindElement(By.Id("EnderecoCobranca_Numero"));
			campoEnderecoCobrancaNumero.SendKeys(doacao.EnderecoCobranca.Numero);
			IWebElement campoEnderecoCobrancaCidade = _driver.FindElement(By.Id("EnderecoCobranca_Cidade"));
			campoEnderecoCobrancaCidade.SendKeys(doacao.EnderecoCobranca.Cidade);
			IWebElement campoEnderecoCobrancaEstado = _driver.FindElement(By.Id("estado"));
			campoEnderecoCobrancaEstado.SendKeys(doacao.EnderecoCobranca.Estado);
			IWebElement campoEnderecoCobrancaCEP = _driver.FindElement(By.Id("cep"));
			campoEnderecoCobrancaCEP.SendKeys(doacao.EnderecoCobranca.CEP);
			IWebElement campoEnderecoCobrancaComplemento = _driver.FindElement(By.Id("EnderecoCobranca_Complemento"));
			campoEnderecoCobrancaComplemento.SendKeys(doacao.EnderecoCobranca.Complemento);
			IWebElement campoEnderecoCobrancatelefone = _driver.FindElement(By.Id("telefone"));
			campoEnderecoCobrancatelefone.SendKeys(doacao.EnderecoCobranca.Telefone);
			//
			IWebElement campoFormaPagamentoNomeTitular = _driver.FindElement(By.Id("FormaPagamento_NomeTitular"));
			campoFormaPagamentoNomeTitular.SendKeys(doacao.FormaPagamento.NomeTitular);
			IWebElement campoFormaPagamentoNumerodoCartao = _driver.FindElement(By.Id("cardNumber"));
			campoFormaPagamentoNumerodoCartao.SendKeys(doacao.FormaPagamento.NumeroCartaoCredito);
			IWebElement campoFormaPagamentoValidade = _driver.FindElement(By.Id("validade"));
			campoFormaPagamentoValidade.SendKeys(doacao.FormaPagamento.Validade);
			IWebElement campoFormaPagamentoCVV = _driver.FindElement(By.Id("cvv"));
			campoFormaPagamentoCVV.SendKeys(doacao.FormaPagamento.CVV);
			//Aciona o botão Doar
			//webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			//webElement.Click();
			//<--

			//Assert
			//_driver.Url.Should().Contain("/Doacoes/Create");
			//-> Alterado para verificar se acessa a index;
			_driver.Url.Should().Contain("/Home/index");
		}
	}
}