import React from "react";
import Header from "../header/Header";
import Footer from "../footer/Footer";

export default function MainLayout({ children }) {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-grow">
        {children}
      </main>
      <Footer />
    </div>
  )
}
