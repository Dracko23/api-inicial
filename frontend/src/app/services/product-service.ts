import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateProduct, Product } from '../models/catalog';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/productos`;

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.endpoint);
  }

  create(product: CreateProduct): Observable<Product> {
    return this.http.post<Product>(this.endpoint, product);
  }

  update(id: number, product: CreateProduct): Observable<Product> {
    return this.http.patch<Product>(`${this.endpoint}/${id}`, product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}
