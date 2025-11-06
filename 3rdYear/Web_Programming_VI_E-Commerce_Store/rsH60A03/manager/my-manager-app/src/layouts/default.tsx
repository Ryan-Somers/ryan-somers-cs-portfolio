import { Navbar } from "@/components/navbar";

export default function DefaultLayout({
                                          children,
                                          onSearch,
                                      }: {
    children: React.ReactNode;
    onSearch?: (term: string) => void;
}) {
    return (
        <div className="relative flex flex-col h-screen">
            <Navbar onSearch={onSearch} />
            <main className="container mx-auto max-w-7xl px-6 flex-grow pt-16">
                {children}
            </main>
            <footer className="w-full flex items-center justify-center py-3">
                <span className="text-default-600">Ryan's</span>
                <p className="text-primary mx-1">Store</p>
            </footer>
        </div>
    );
}
