# API BookStore

Este repositório contém o código-fonte de uma API ASP.NET Core responsável por gerenciar livros em uma aplicação de livraria. A API utiliza caching para otimizar a recuperação e manipulação dos dados dos livros.

## Índice

- [Introdução](#introdução)
- [Endpoints](#endpoints)
  - [GetBooks](#GetBooks)
  - [GetBook](#GetBook)
  - [UpdateBook](#UpdateBook)
  - [AddBook](#AddBook)
  - [DeleteBook](#DeleteBook)
- [Caching](#caching)
- [Conclusão](#conclusão)

## Introdução

O `BooksController` faz parte da API BookStore, que tem como objetivo fornecer endpoints para gerenciar dados relacionados a livros. Este controlador interage com um banco de dados SQL Server usando o Entity Framework Core e implementa mecanismos de caching do Redis para aprimorar o desempenho.

## Endpoints

### GetBooks

- **Método HTTP:** GET
- **Rota:** /api/Books
- **Descrição:** Recupera uma lista de todos os livros.

### GetBook

- **Método HTTP:** GET
- **Rota:** /api/Books/{id}
- **Descrição:** Recupera os detalhes de um livro específico identificado por seu `id`.

### UpdateBook

- **Método HTTP:** PUT
- **Rota:** /api/Books/{id}
- **Descrição:** Atualiza os detalhes de um livro específico identificado por seu `id`. Aceita um objeto `Book` modificado no corpo da solicitação.

### AddBook

- **Método HTTP:** POST
- **Rota:** /api/Books
- **Descrição:** Cria um novo livro. Espera um objeto `Book` no corpo da solicitação.

### DeleteBook

- **Método HTTP:** DELETE
- **Rota:** /api/Books/{id}
- **Descrição:** Exclui um livro com o `id` especificado.

## Caching

O controlador incorpora o caching usando a classe `CacheService` para melhorar o desempenho. O caching é aplicado nos seguintes cenários:

- Ao recuperar todos os livros (método `GetBooks`), a lista é armazenada em cache usando a chave `"Books:All"`.
- Ao recuperar um livro específico (método `GetBook`), o livro é armazenado em cache usando a chave `"Book:{id}"`.
- Ao modificar ou adicionar um livro (métodos `UpdateBook`, `AddBook`), as entradas de cache correspondentes são atualizadas ou removidas.

## Conclusão

Em conclusão, a API BookStore apresenta uma solução robusta e eficiente para o gerenciamento de livros em uma aplicação de livraria. Utilizando o framework ASP.NET Core, o BooksController interage de forma eficaz com um banco de dados SQL Server por meio do Entity Framework Core, proporcionando operações fundamentais de CRUD (Create, Read, Update, Delete) através de endpoints bem definidos.

A implementação de caching, utilizando o Redis, destaca-se como uma estratégia inteligente para otimizar o desempenho da API. O armazenamento em cache é aplicado de maneira estratégica, sendo utilizado tanto ao recuperar a lista completa de livros quanto ao acessar detalhes específicos de um livro. Essa abordagem contribui significativamente para reduzir a carga no banco de dados e melhorar a responsividade da aplicação.

Os endpoints da API (GetBooks, GetBook, UpdateBook, AddBook e DeleteBook) são claramente definidos, seguindo as melhores práticas de design de API. A documentação fornecida facilita a compreensão e utilização da API, tornando-a acessível para desenvolvedores.

Em resumo, a API BookStore não apenas atende às necessidades básicas de gerenciamento de livros, mas também demonstra um cuidado especial com o desempenho por meio da implementação de caching. Isso a torna uma escolha sólida para aplicações que buscam eficiência na manipulação de dados relacionados a livros.
