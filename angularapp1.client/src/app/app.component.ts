import { Component, OnInit } from '@angular/core'; 
import { HttpClient } from '@angular/common/http'; 

interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  createdDate: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})

export class AppComponent implements OnInit {

  public products: Product[] = [];

  constructor(private readonly http: HttpClient) { }

  ngOnInit() {
    this.GetProducts();
  }

  title = 'ClientApp';

  private GetProducts() {
    this.http.get<Product[]>('https://localhost:7157/api/Product').subscribe({
      next: (result) => {
        this.products = result;
      },
      error: (error) => console.error(error)
    });

  }

}
