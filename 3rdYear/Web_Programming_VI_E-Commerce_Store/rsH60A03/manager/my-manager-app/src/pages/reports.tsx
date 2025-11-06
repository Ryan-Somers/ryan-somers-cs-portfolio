import {useState} from "react";
import {fetchOrdersByDate, fetchOrdersByCustomer, OrderType} from "@/api/myApi";
import {Table, TableHeader, TableColumn, TableBody, TableRow, TableCell} from "@nextui-org/table";
import {Input} from "@nextui-org/input";
import {Button} from "@nextui-org/button";
import DefaultLayout from "@/layouts/default";

export default function ReportsPage() {
    const [date, setDate] = useState<string>("");
    const [customerId, setCustomerId] = useState<number | null>(null);
    const [orders, setOrders] = useState<OrderType[]>([]);
    const [total, setTotal] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const handleFetchByDate = async () => {
        setLoading(true);
        try {
            const {orders, total} = await fetchOrdersByDate(date);
            setOrders(orders);
            setTotal(total);
            setError(null);
        } catch (e) {
            console.error(e);
            setError("Failed to fetch orders by date.");
        } finally {
            setLoading(false);
        }
    };

    const handleFetchByCustomer = async () => {
        setLoading(true);
        try {
            const {orders, total} = await fetchOrdersByCustomer(customerId!);
            setOrders(orders);
            setTotal(total);
            setError(null);
        } catch (e) {
            console.error(e);
            setError("Failed to fetch orders by customer.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <DefaultLayout>
            <div className="p-4">
                <h1 className="text-2xl font-bold">Reports</h1>
                <div className="flex flex-col gap-4 mt-6">
                    <div>
                        <Input
                            type="date"
                            label="Enter Date"
                            value={date}
                            onChange={(e) => setDate(e.target.value)}
                        />
                        <Button onClick={handleFetchByDate} isDisabled={!date} className="mt-2">
                            Fetch Orders by Date
                        </Button>
                    </div>

                    <div>
                        <Input
                            type="number"
                            label="Enter Customer ID"
                            value={customerId || ""}
                            onChange={(e) => setCustomerId(parseInt(e.target.value))}
                        />
                        <Button onClick={handleFetchByCustomer} isDisabled={!customerId} className="mt-2">
                            Fetch Orders by Customer
                        </Button>
                    </div>
                </div>

                {loading && <p>Loading...</p>}
                {error && <p className="text-red-500">{error}</p>}

                {!loading && !error && (
                    <>
                        <h2 className="text-xl font-semibold mt-6 mb-6">Orders</h2>
                        <Table>
                            <TableHeader>
                                <TableColumn>Order ID</TableColumn>
                                <TableColumn>Date Created</TableColumn>
                                <TableColumn>Customer ID</TableColumn>
                                <TableColumn>Total</TableColumn>
                            </TableHeader>
                            <TableBody emptyContent={"No Orders to display!"}>
                                {orders.map((order) => (
                                    <TableRow key={order.orderId}>
                                        <TableCell>{order.orderId}</TableCell>
                                        <TableCell>{new Date(order.dateCreated).toLocaleDateString()}</TableCell>
                                        <TableCell>{order.customerId}</TableCell>
                                        <TableCell>{order.total.toFixed(2)}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                        <p className="mt-4 font-semibold">Total of Orders: ${total.toFixed(2)}</p>
                    </>
                )}

            </div>
        </DefaultLayout>
    );

}
