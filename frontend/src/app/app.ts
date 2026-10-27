import { CurrencyPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import { CategoryFormComponent } from './components/category-form/category-form';
import { ProductFormComponent } from './components/product-form/product-form';
import { Category, CreateProduct, Product } from './models/catalog';
import { StockStatusPipe } from './pipes/stock-status-pipe';
import { CategoryService } from './services/category-service';
import { ProductService } from './services/product-service';
import { LowStockDirective } from './shared/low-stock-directive';

@Component({
  imports: [
    CategoryFormComponent,
    CurrencyPipe,
    DatePipe,
    LowStockDirective,
    ProductFormComponent,
    StockStatusPipe,
  ],
  selector: 'app-root',
  templateUrl: './app.html',
})
export class App {
  private readonly categoryService = inject(CategoryService);
  private readonly productService = inject(ProductService);

  protected readonly products = signal<Product[]>([]);
  protected readonly categories = signal<Category[]>([]);
  protected readonly loading = signal(true);
  protected readonly connected = signal(false);
  protected readonly error = signal('');
  protected readonly search = signal('');
  protected readonly categoryFilter = signal('all');
  protected readonly statusFilter = signal('all');
  protected readonly activeDialog = signal<'product' | 'category' | null>(null);
  protected readonly editingProduct = signal<Product | null>(null);
  protected readonly today = new Date();
  protected readonly filteredProducts = computed(() => {
    const query = this.search().trim().toLocaleLowerCase();
    const category = this.categoryFilter();
    const status = this.statusFilter();

    return this.products().filter((product) => {
      const matchesSearch =
        !query ||
        product.nombre.toLocaleLowerCase().includes(query) ||
        product.nombreCategoria.toLocaleLowerCase().includes(query);
      const matchesCategory =
        category === 'all' || product.categoriaId === Number(category);
      const matchesStatus =
        status === 'all' ||
        (status === 'active' ? product.estado : !product.estado);

      return matchesSearch && matchesCategory && matchesStatus;
    });
  });
  protected readonly activeProducts = computed(
    () => this.products().filter((product) => product.estado).length,
  );
  protected readonly lowStockProducts = computed(
    () => this.products().filter((product) => product.stock < 5).length,
  );
  protected readonly inventoryValue = computed(() =>
    this.products().reduce(
      (total, product) => total + product.precio * product.stock,
      0,
    ),
  );

  constructor() {
    this.loadCatalog();
  }

  protected loadCatalog(): void {
    this.loading.set(true);
    this.error.set('');

    forkJoin({
      products: this.productService.getAll(),
      categories: this.categoryService.getAll(),
    }).subscribe({
      next: ({ products, categories }) => {
        this.products.set(products);
        this.categories.set(categories);
        this.connected.set(true);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.error.set(this.getErrorMessage(error));
        this.connected.set(false);
        this.loading.set(false);
      },
    });
  }

  protected openNewProduct(): void {
    this.editingProduct.set(null);
    this.activeDialog.set('product');
  }

  protected setSearch(event: Event): void {
    if (event.target instanceof HTMLInputElement) {
      this.search.set(event.target.value);
    }
  }

  protected setCategoryFilter(event: Event): void {
    if (event.target instanceof HTMLSelectElement) {
      this.categoryFilter.set(event.target.value);
    }
  }

  protected setStatusFilter(event: Event): void {
    if (event.target instanceof HTMLSelectElement) {
      this.statusFilter.set(event.target.value);
    }
  }

  protected openEditProduct(product: Product): void {
    this.editingProduct.set(product);
    this.activeDialog.set('product');
  }

  protected closeDialog(): void {
    this.activeDialog.set(null);
    this.editingProduct.set(null);
  }

  protected saveProduct(product: CreateProduct): void {
    const existingProduct = this.editingProduct();
    const request = existingProduct
      ? this.productService.update(existingProduct.id, product)
      : this.productService.create(product);

    request.subscribe({
      next: () => {
        this.closeDialog();
        this.loadCatalog();
      },
      error: (error: unknown) => this.error.set(this.getErrorMessage(error)),
    });
  }

  protected saveCategory(category: Omit<Category, 'id'>): void {
    this.categoryService.create(category).subscribe({
      next: () => {
        this.closeDialog();
        this.loadCatalog();
      },
      error: (error: unknown) => this.error.set(this.getErrorMessage(error)),
    });
  }

  protected deleteProduct(product: Product): void {
    if (!window.confirm(`¿Eliminar "${product.nombre}" del inventario?`)) {
      return;
    }

    this.productService.delete(product.id).subscribe({
      next: () => this.loadCatalog(),
      error: (error: unknown) => this.error.set(this.getErrorMessage(error)),
    });
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      const body: unknown = error.error;
      if (typeof body === 'object' && body !== null && 'mensaje' in body) {
        const message = body.mensaje;
        if (typeof message === 'string') {
          return message;
        }
      }

      if (error.status === 0) {
        return 'No se pudo conectar con la API. Comprueba que el backend esté activo y que el certificado HTTPS sea de confianza.';
      }

      return error.message || 'Ocurrió un error al comunicarse con la API.';
    }

    return 'Ocurrió un error inesperado.';
  }
}
