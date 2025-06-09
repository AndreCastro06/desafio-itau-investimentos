import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import Relatorios from "./pages/Relatorios";
import { BrowserRouter, Routes, Route,} from 'react-router-dom'
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <div className="p-4 bg-white shadow">
       <h1 className="mr-4  font-semibold text-blue-500 font-semibold">Dashboard Processo Seletivo Itaú Unibanco</h1>
      </div>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="/relatorios" element={<Relatorios />} />
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
)