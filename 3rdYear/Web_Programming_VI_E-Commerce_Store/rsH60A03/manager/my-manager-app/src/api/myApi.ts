import apiClient from "@/api/apiClient.ts";

export interface ProductType {
  productId: number;
  prodCatId: number;
  description: string;
  manufacturer: string;
  stock: number;
  buyPrice: number;
  sellPrice: number;
  imageUrl: string;
}

export interface OrderType {
  orderId: number;
  customerId: number;
  dateCreated: string;
  dateFulfilled?: string | null;
  total: number;
  taxes?: number;
  orderItems?: OrderItemType[];
}

export interface OrderItemType {
  orderItemId: number;
  orderId: number;
  productId: number;
  quantity: number;
  price: number;
  productName?: string;
}

export interface CategoryType {
  categoryId: number;
  prodCat: string;
}

export const fetchProducts = async (): Promise<ProductType[]> => {
  try {
    const response = await apiClient.get<ProductType[]>("/api/products");
    return response.data
  }catch (e) {
    console.log("Error fetching data", e);
    throw e;
  }
};

export const fetchProductById = async (productId: number): Promise<ProductType> => {
  try {
    const response = await apiClient.get<ProductType>(`/api/products/${productId}`);
    return response.data;
  } catch (e) {
    console.error("Error fetching product by ID", e);
    throw e;
  }
};


export const fetchCategories = async (): Promise<CategoryType[]> => {
  try {
    const response = await apiClient.get<CategoryType[]>("/api/productcategory");
    return response.data;
  } catch (e) {
    console.log("Error fetching categories", e);
    throw e;
  }
};

export const fetchProductsByCategory = async (categoryId: number): Promise<ProductType[]> => {
  try {
    const response = await apiClient.get<ProductType[]>(`/api/products/category/${categoryId}`);
    return response.data
  }catch (e) {
    console.log("Error fetching data", e);
    throw e;
  }
};

export const searchProductsByName = async (productName: string): Promise<ProductType[]> => {
  try {
    const response = await apiClient.get<ProductType[]>(`/api/products/search/${productName}`);
    return response.data
  }catch (e) {
    console.log("Error fetching data", e);
    throw e;
  }
};

export const fetchProductsSortedByBuyPrice = async (ascending: boolean): Promise<ProductType[]> => {
  const response = await apiClient.get<ProductType[]>(`/api/products/SortByBuyPrice?ascending=${ascending}`);
  return response.data;
};

export const fetchProductsSortedBySellPrice = async (ascending: boolean): Promise<ProductType[]> => {
  const response = await apiClient.get<ProductType[]>(`/api/products/SortBySellPrice?ascending=${ascending}`);
  return response.data;
};

export const fetchProductsSortedByStock = async (ascending: boolean): Promise<ProductType[]> => {
  const response = await apiClient.get<ProductType[]>(`/api/products/SortByStock?ascending=${ascending}`);
  return response.data;
};

export const fetchOrdersByDate = async (date: string): Promise<{ orders: OrderType[]; total: number }> => {
  const response = await apiClient.get(`/api/order/ByDate/${date}`);
  return response.data;
};

export const fetchOrdersByCustomer = async (customerId: number): Promise<{ orders: OrderType[]; total: number }> => {
  const response = await apiClient.get(`/api/order/ByCustomer/${customerId}`);
  return response.data;
};


export const updateProduct = async (product: ProductType): Promise<void> => {
    await apiClient.put(`/api/products/${product.productId}`, product);
};


