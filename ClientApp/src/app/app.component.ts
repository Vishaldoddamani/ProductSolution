import { Component,OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common'; 

interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  createdDate: string;
}

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule], 
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})

export class AppComponent implements OnInit {

  public products: Product[] = [];

  constructor(private readonly http: HttpClient) { }

  ngOnInit() {
    this.GetProducts();
  } 

  title = 'ClientApp';

  private GetProducts() {
   this.http.get<Product[]>('/api/products').subscribe({
     next: (result) => {
       this.products = result;
     },
     error: (error) => console.error(error)
   });

  }

}
