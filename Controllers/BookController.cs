using LearnAPI.Data;
using LearnAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Controllers
{
    [Route("/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApiContext _context;

        public BookController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult AddBook(Book newBook)
        {
            Response.ContentType = "application/json; charset=utf-8";
            if (newBook.name == null)
            {
                Response.StatusCode = 400;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Gagal menambahkan buku. Mohon isi nama buku"
                    }
                    );
            }

            if (newBook.readPage > newBook.pageCount)
            {
                Response.StatusCode = 400;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Gagal menambahkan buku. readPage tidak boleh lebih besar dari pageCount"
                    }
                    );
            }
            //newBook.id = _context.BookData.ToArray().Length;
            newBook.finished = newBook.readPage == newBook.pageCount;
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            newBook.insertedAt = currentTime;
            newBook.updatedAt = currentTime;

            _context.BookData.Add(newBook);
            _context.SaveChanges();

            Response.StatusCode = 201;
            return new JsonResult(
                new {
                    status = "success",
                    message = "Buku berhasil ditambahkan",
                    data = new { bookId = newBook.id }
                }
                );
        }

        [HttpGet]
        public JsonResult getBooks(string? name, string? reading, string? finished)
        {
            Response.ContentType = "application/json; charset=utf-8";
            Book[] books = _context.BookData.ToArray();
            List<object> mappedBooks = new List<object>();
            for (int i = 0; i < books.Length; i++)
            {
                if ((name == null || books[i].name.ToLower().Contains(name.ToLower())) && (reading == null || (reading == "1") == books[i].reading) && (finished == null || (finished == "1") == books[i].finished))
                {
                    mappedBooks.Add(new
                    {
                        id = books[i].id,
                        name = books[i].name,
                        publisher = books[i].publisher,
                    });
                }
            }
            Response.StatusCode = 200;
            return new JsonResult(
                new
                {
                    status = "success",
                    data = new { books = mappedBooks }
                }
                );
        }

        [HttpGet("{id}")]
        public JsonResult getBookDetails(String id)
        {
            Response.ContentType = "application/json; charset=utf-8";
            Book selectedBook;
            try
            {
                int id2 = int.Parse(id);
                selectedBook = _context.BookData.Find(id2);
                if (selectedBook == null)
                    throw new Exception();
            } catch (Exception e)
            {
                Response.StatusCode = 404;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Buku tidak ditemukan"
                    }
                    ); 
            }
            Response.StatusCode = 200;
            return new JsonResult(
                new
                {
                    status = "success",
                    data = new { book = selectedBook }
                }
                );
        }

        [HttpPut("{id}")]
        public JsonResult updateBookDetails(String id, Book newBook)
        {
            Response.ContentType = "application/json; charset=utf-8";
            if (newBook.name == null)
            {
                Response.StatusCode = 400;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Gagal memperbarui buku. Mohon isi nama buku"
                    }
                    );
            }
            if (newBook.readPage > newBook.pageCount)
            {
                Response.StatusCode = 400;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Gagal memperbarui buku. readPage tidak boleh lebih besar dari pageCount"
                    }
                    );
            }
            Book selectedBook;
            try
            {
                int id2 = int.Parse(id);
                newBook.id = id2;
                _context.BookData.Update(newBook);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Response.StatusCode = 404;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Gagal memperbarui buku. Id tidak ditemukan"
                    }
                    );
            }
            Response.StatusCode = 200;
            return new JsonResult(
                new
                {
                    status = "success",
                    message = "Buku berhasil diperbarui"
                }
                );
        }

        [HttpDelete("{id}")]
        public JsonResult deleteBook(String id)
        {
            Response.ContentType = "application/json; charset=utf-8";
            Book selectedBook;
            try
            {
                int id2 = int.Parse(id);
                _context.BookData.Remove(_context.BookData.Find(id2));
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Response.StatusCode = 404;
                return new JsonResult(
                    new
                    {
                        status = "fail",
                        message = "Buku gagal dihapus. Id tidak ditemukan"
                    }
                    );
            }
            Response.StatusCode = 200;
            return new JsonResult(
                new
                {
                    status = "success",
                    message = "Buku berhasil dihapus"
                }
                );
        }
    }
}
