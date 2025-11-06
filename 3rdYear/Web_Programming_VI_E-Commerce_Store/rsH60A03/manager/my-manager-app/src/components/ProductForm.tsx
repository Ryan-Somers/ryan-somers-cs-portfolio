import React, {useState, useEffect} from "react";
import {Form, Input, Button, Modal, ModalContent, ModalHeader, ModalBody, ModalFooter} from "@nextui-org/react";
import {updateProduct, ProductType, fetchProductById} from "@/api/myApi.ts";

export default function ProductForm({
                                        productId,
                                        onClose,
                                    }: {
    productId: number;
    onClose: () => void; // Callback to reset state in the parent component
}) {
    const [product, setProduct] = useState<ProductType | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [errors, setErrors] = useState<{ [key: string]: string }>({});
    const [modalVisible, setModalVisible] = useState<boolean>(false); // Modal state

    useEffect(() => {
        const loadProduct = async () => {
            setLoading(true);
            try {
                const selectedProduct = await fetchProductById(productId);
                setProduct(selectedProduct);
            } catch (e) {
                console.error("Error loading product:", e);
                setErrors({fetch: "Failed to load product data."});
            } finally {
                setLoading(false);
            }
        };

        loadProduct();
    }, [productId]);

    const validate = () => {
        const newErrors: { [key: string]: string } = {};
        if (product?.sellPrice <= product?.buyPrice) {
            newErrors.sellPrice = "Sell price must be higher than buy price.";
            newErrors.buyPrice = "Buy price must be lower than sell price.";
        }
        if (product?.stock < 0) {
            newErrors.stock = "Stock cannot be negative.";
        }
        return newErrors;
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!product) return;

        const validationErrors = validate();
        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors);
            return;
        }

        try {
            const formData = new FormData(e.currentTarget);
            const updatedProduct: ProductType = {
                ...product,
                stock: parseInt(formData.get("stock") as string, 10),
                buyPrice: parseFloat(formData.get("buyPrice") as string),
                sellPrice: parseFloat(formData.get("sellPrice") as string),
            };

            await updateProduct(updatedProduct);
            setErrors({}); // Clear validation errors
            setModalVisible(true); // Open modal
        } catch (e) {
            console.error("Error updating product:", e);
            setErrors({update: "Failed to update product."});
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (errors.fetch) {
        return <div className="text-red-500">{errors.fetch}</div>;
    }

    return (
        <>
            <Form
                className="w-full max-w-sm"
                validationBehavior="native"
                onSubmit={handleSubmit}
            >
                <Input
                    label="Stock"
                    labelPlacement="outside"
                    name="stock"
                    type="number"
                    isRequired
                    value={product?.stock || ""}
                    onChange={(e) =>
                        setProduct((prev) => prev && {...prev, stock: +e.target.value})
                    }
                    errorMessage={errors.stock} // Show error message for stock
                />
                <Input
                    label="Buy Price"
                    labelPlacement="outside"
                    name="buyPrice"
                    type="number"
                    isRequired
                    step="0.01"
                    value={product?.buyPrice || ""}
                    onChange={(e) =>
                        setProduct((prev) => prev && {...prev, buyPrice: +e.target.value})
                    }
                    errorMessage={errors.buyPrice} // Show error message for buy price
                />
                <Input
                    label="Sell Price"
                    labelPlacement="outside"
                    name="sellPrice"
                    type="number"
                    isRequired
                    step="0.01"
                    value={product?.sellPrice || ""}
                    onChange={(e) =>
                        setProduct((prev) => prev && {...prev, sellPrice: +e.target.value})
                    }
                    errorMessage={errors.sellPrice} // Show error message for sell price
                />
                <div className="flex flex-row gap-2">
                <Button type="submit" variant="bordered" className="mt-4">
                    Update Product
                </Button>
                </div>
            </Form>

            <Modal isOpen={modalVisible} onClose={() => setModalVisible(false)}>
                <ModalContent>
                    {(closeModal) => (
                        <>
                            <ModalHeader className="flex flex-col gap-1">Success!</ModalHeader>
                            <ModalBody>
                                <p>
                                    {`You have successfully updated the product: "${product?.description}".`}
                                </p>
                            </ModalBody>
                            <ModalFooter>
                                <Button
                                    color="primary"
                                    onPress={() => {
                                        closeModal(); // Closes the modal
                                        setModalVisible(false); // Updates the modal state
                                        onClose(); // Calls the parent callback to reset state
                                    }}
                                >
                                    Back to Products
                                </Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>

        </>
    );
}
