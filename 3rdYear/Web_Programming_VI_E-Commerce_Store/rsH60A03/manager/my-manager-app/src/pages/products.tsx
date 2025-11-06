import {useEffect, useState} from "react";
import {
    Table,
    TableHeader,
    TableBody,
    TableColumn,
    TableRow,
    TableCell,
} from "@nextui-org/table";
import {
    Dropdown,
    DropdownTrigger,
    DropdownMenu,
    DropdownItem,
} from "@nextui-org/dropdown";
import {Button} from "@nextui-org/button";

import {
    fetchProducts,
    searchProductsByName,
    ProductType,
    fetchProductsByCategory,
    fetchCategories,
    CategoryType,
    fetchProductsSortedByBuyPrice,
    fetchProductsSortedBySellPrice,
    fetchProductsSortedByStock,
    updateProduct,
} from "@/api/myApi.ts";
import DefaultLayout from "@/layouts/default";
import {EditIcon} from "@/components/icons.tsx";
import {Tooltip} from "@nextui-org/react";
import ProductForm from "@/components/ProductForm";

export default function ProductsPage() {
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);
    const [categories, setCategories] = useState<CategoryType[]>([]);
    const [product, setProduct] = useState<ProductType[]>([]);
    const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
    const [sortedColumn, setSortedColumn] = useState<string | null>(null);
    const [ascending, setAscending] = useState<boolean>(true);
    const [editProduct, setEditProduct] = useState<ProductType | null>(null);

    const fetchData = async (searchTerm: string | null = null) => {
        setLoading(true);
        try {
            const result = searchTerm
                ? await searchProductsByName(searchTerm)
                : await fetchProducts();

            setProduct(result);
        } catch (e) {
            console.error("Error fetching products:", e);
            setError("Failed to fetch products.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    useEffect(() => {
        const getCategories = async () => {
            try {
                const result = await fetchCategories();

                setCategories(result);
            } catch (e) {
                console.error("Error fetching categories:", e);
                setError("Failed to fetch categories.");
            }
        };

        getCategories();
    }, []);

    const handleSearch = (term: string) => {
        fetchData(term);
    };

    const handleCategorySelect = async (categoryId: string) => {
        setSelectedCategory(categoryId);
        setLoading(true);

        try {
            const result = await fetchProductsByCategory(parseInt(categoryId));

            setProduct(result);
        } catch (e) {
            console.error("Error fetching products by category:", e);
            setError("Failed to fetch products by category.");
        } finally {
            setLoading(false);
        }
    };

    const handleSort = async (key: string) => {
        setLoading(true);
        try {
            let sortedData: ProductType[] = [];

            switch (key) {
                case "buyPrice":
                    sortedData = await fetchProductsSortedByBuyPrice(ascending);
                    break;
                case "sellPrice":
                    sortedData = await fetchProductsSortedBySellPrice(ascending);
                    break;
                case "stock":
                    sortedData = await fetchProductsSortedByStock(ascending);
                    break;
                default:
                    console.warn(`Unsupported sorting key: ${key}`);
                    break;
            }

            setProduct(sortedData);
            setSortedColumn(key);
            setAscending(!ascending); // Toggle sorting direction
        } catch (e) {
            console.error(`Error sorting by ${key}:`, e);
            setError(`Failed to sort products by ${key}.`);
        } finally {
            setLoading(false);
        }
    };

    const handleEditClick = (product: ProductType) => {
        setEditProduct(product); // Set the product to be edited
    };

    const handleFormSubmit = async (updatedProduct: ProductType) => {
        try {
            await updateProduct(updatedProduct); // Update the product
            setEditProduct(null); // Close the form
            fetchData(); // Refresh the product list
        } catch (e) {
            console.error("Error updating product:", e);
            setError("Failed to update product.");
        }
    };

    return (
        <DefaultLayout onSearch={handleSearch}>
            {editProduct ? (
                <ProductForm
                    productId={editProduct.productId}
                    onSubmit={handleFormSubmit}
                    onClose={() => {
                        setEditProduct(null); // Reset the form view
                        fetchData(); // Refresh the product list
                    }}
                    onCancel={() => setEditProduct(null)}
                />

            ) : (
                <>
                    <section className="flex flex-col gap-4 py-8 md:py-10">
                        <div className="flex flex-row items-start gap-3">
                            <h1 className="font-semibold text-3xl">All Products</h1>
                            <Dropdown backdrop="blur">
                                <DropdownTrigger>
                                    <Button variant="bordered">Select Category</Button>
                                </DropdownTrigger>
                                <DropdownMenu
                                    aria-label="Category selection"
                                    variant="faded"
                                    onAction={(key) => handleCategorySelect(key as string)}
                                >
                                    {categories.map((category) => (
                                        <DropdownItem key={category.categoryId}>
                                            {category.prodCat}
                                        </DropdownItem>
                                    ))}
                                </DropdownMenu>
                            </Dropdown>
                        </div>
                    </section>
                    <Table>
                        <TableHeader>
                            <TableColumn>Description</TableColumn>
                            <TableColumn>Manufacturer</TableColumn>
                            <TableColumn
                                allowsSorting
                                isSorted={sortedColumn === "stock"}
                                sortDirection={ascending ? "ascending" : "descending"}
                                onClick={() => handleSort("stock")}
                            >
                                Stock
                            </TableColumn>
                            <TableColumn
                                allowsSorting
                                isSorted={sortedColumn === "buyPrice"}
                                sortDirection={ascending ? "ascending" : "descending"}
                                onClick={() => handleSort("buyPrice")}
                            >
                                Buy Price
                            </TableColumn>
                            <TableColumn
                                allowsSorting
                                isSorted={sortedColumn === "sellPrice"}
                                sortDirection={ascending ? "ascending" : "descending"}
                                onClick={() => handleSort("sellPrice")}
                            >
                                Sell Price
                            </TableColumn>
                            <TableColumn>Actions</TableColumn>
                        </TableHeader>
                        <TableBody emptyContent={"No rows to display."}>
                            {product.map((item) => (
                                <TableRow key={item.productId}>
                                    <TableCell>{item.description}</TableCell>
                                    <TableCell>{item.manufacturer}</TableCell>
                                    <TableCell>{item.stock}</TableCell>
                                    <TableCell>{item.buyPrice}</TableCell>
                                    <TableCell>{item.sellPrice}</TableCell>
                                    <TableCell>
                                        <div className="relative flex items-center gap-2">
                                            <Tooltip content="Edit Product">
                        <span
                            className="text-lg text-default-400 cursor-pointer active:opacity-50"
                            onClick={() => handleEditClick(item)}
                        >
                          <EditIcon/>
                        </span>
                                            </Tooltip>
                                        </div>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </>
            )}
        </DefaultLayout>
    );
}
