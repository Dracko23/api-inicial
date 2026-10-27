export interface Product {
  id: number;
  nombre: string;
  precio: number;
  stock: number;
  estado: boolean;
  categoriaId: number;
  nombreCategoria: string;
}

export interface Category {
  id: number;
  nombre: string;
  descripcion: string | null;
}

export interface CreateProduct {
  nombre: string;
  precio: number;
  stock: number;
  estado: boolean;
  categoriaId: number;
}

export interface CreateCategory {
  nombre: string;
  descripcion: string | null;
}
