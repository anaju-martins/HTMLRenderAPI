# HTML Render API 🖼️📄

Uma API poderosa e simples construída em .NET para renderizar HTML como imagem ou PDF, retornando o resultado em Base64. Ideal para gerar relatórios, cartões, vouchers ou qualquer conteúdo visual dinamicamente a partir de código HTML.

## ✨ Funcionalidades Principais

-   **HTML para Imagem**: Converta qualquer estrutura HTML em uma imagem de alta qualidade (JPEG).
-   **HTML para PDF**: Gere documentos PDF a partir do seu código HTML.
-   **Fácil Integração**: Receba o arquivo renderizado como uma string Base64, pronto para ser usado em qualquer aplicação web ou móvel.
-   **Motor de Renderização Robusto**: Utiliza o **Puppeteer Sharp**, uma biblioteca .NET que controla um navegador headless (Chromium), garantindo que a renderização seja idêntica à do Google Chrome.
-   **Customização via HTML/CSS**: Todo o estilo e layout são controlados pelo HTML e CSS que você envia, oferecendo total flexibilidade.

---

## 🚀 Tecnologias Utilizadas

-   **[.NET 8](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)**: Plataforma de desenvolvimento principal.
-   **[ASP.NET Core](https://learn.microsoft.com/pt-br/aspnet/core/)**: Framework para construção da API.
-   **[Puppeteer Sharp](https://www.puppeteersharp.com/)**: Biblioteca para controle do navegador headless (Chromium).

---

## 💡 Como Usar

Para utilizar a API, envie uma requisição `POST` para o endpoint `/Render` com um corpo JSON contendo o HTML a ser renderizado e o formato de saída desejado.

**Endpoint:** `POST /Render`

### Corpo da Requisição (Request Body)

O corpo da requisição deve ser um JSON com a seguinte estrutura:

```json
{
  "html": "string",
  "format": 0 para Jped e 1 para Pdf
}
