import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Book {
  category: string;
  isbn: string;
  title: string;
  authors: string[];
  year: number;
  price: number;
}

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  private apiUrl = 'http://localhost:5046/books';

  constructor(private http: HttpClient) {}

  getBooks() {
    return this.http.get<{ books: Book[] }>(this.apiUrl);
  }

  addBook(book: Book): Observable<any> {
    return this.http.post<any>(`http://localhost:5046/books`, book, {
      headers: {
        'Content-Type': 'application/json',
      },
    });
  }

  updateBook(book: Book) {
    return this.http.put(`http://localhost:5046/books/${book.isbn}`, book);
  }

  deleteBook(isbn: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${isbn}`);
  }
}
