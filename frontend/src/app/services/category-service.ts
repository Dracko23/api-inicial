import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category, CreateCategory } from '../models/catalog';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/categorias`;

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.endpoint);
  }

  create(category: CreateCategory): Observable<Category> {
    return this.http.post<Category>(this.endpoint, category);
  }
}
