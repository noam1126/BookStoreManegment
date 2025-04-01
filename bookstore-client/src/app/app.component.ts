import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BooksService } from './services/books.service';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';

export interface Book {
  category: string;
  isbn: string;
  title: string;
  authors: string[];
  year: number;
  price: number;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  books: Book[] = [];
  isDialogOpen = false;
  isEditMode = false;
  currentBook: any = {
    category: '',
    isbn: '',
    title: '',
    authors: '',
    year: 0,
    price: 0,
  };

  constructor(private booksService: BooksService) {}

  ngOnInit(): void {
    this.fetchBooks();
  }

  fetchBooks() {
    this.booksService.getBooks().subscribe({
      next: (data) => {
        console.log('Books data:', data);
        if (data && Array.isArray(data)) {
          this.books = data.map((book: any) => ({
            category: book.category,
            isbn: book.isbn,
            title: book.title,
            authors: book.authors,
            year: book.year,
            price: book.price,
          }));
        } else {
          console.error(
            'Expected an object with a "books" array, but got:',
            data
          );
        }
      },
      error: (error) => {
        console.error('Error fetching books', error);
      },
    });
  }

  authorsAsString(authors: string[]) {
    return authors?.join(',');
  }
  openAddBookDialog() {
    this.isEditMode = false;
    this.isDialogOpen = true;
    this.currentBook = {
      category: '',
      isbn: '',
      title: '',
      authors: [],
      year: 0,
      price: 0,
    };
  }

  openEditBookDialog(book: Book) {
    this.isEditMode = true;
    this.isDialogOpen = true;
    this.currentBook = { ...book };
  }

  closeDialog() {
    this.isDialogOpen = false;
  }

  addBook() {
    const bookData = {
      category: this.currentBook.category,
      isbn: this.currentBook.isbn,
      title: this.currentBook.title,
      authors: Array.isArray(this.currentBook.authors)
        ? this.currentBook.authors
        : this.currentBook.authors?.split(','),
      year: this.currentBook.year,
      price: this.currentBook.price,
    };

    this.booksService.addBook(bookData).subscribe({
      next: (response) => {
        console.log('Book added successfully', response);

        this.books.push(bookData);
      },
      error: (error) => {
        console.log('Error adding book', error);
      },
    });
  }

  editBook() {
    if (!this.currentBook.isbn) {
      console.error('ISBN is missing!');
      return;
    }

    const bookData = {
      category: this.currentBook.category,
      isbn: this.currentBook.isbn,
      title: this.currentBook.title,
      authors: Array.isArray(this.currentBook.authors)
        ? this.currentBook.authors
        : this.currentBook.authors?.split(','),
      year: this.currentBook.year,
      price: this.currentBook.price,
    };

    this.booksService.updateBook(bookData).subscribe({
      next: (response) => {
        console.log('Book updated successfully:', response);

        let index = this.books.findIndex(
          (b) => b.isbn === this.currentBook.isbn
        );
        if (index !== -1) this.books[index] = { ...bookData };

        this.closeDialog();
      },
      error: (error) => {
        console.error('Error updating book', error);
      },
    });
  }

  deleteBook(Isbn: string) {
    this.booksService.deleteBook(Isbn).subscribe({
      next: (response) => {
        console.log('Book deleted successfully:', response);
        this.books = this.books.filter((book) => book.isbn !== Isbn);
      },
      error: (error) => {
        console.error('Error deleting book', error);
      },
    });
  }
}
