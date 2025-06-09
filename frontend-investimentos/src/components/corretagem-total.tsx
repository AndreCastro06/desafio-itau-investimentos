// src/components/CorretagemTotalChart.tsx
import { useEffect, useState } from "react";
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid, Legend } from "recharts";

interface CorretagemData {
  usuario: string;
  totalCorretagem: number;
}

export default function CorretagemTotalChart() {
  const [data, setData] = useState<CorretagemData[]>([]);

  useEffect(() => {
    fetch("https://localhost:5183/api/relatorios/corretagem-total")
      .then((res) => res.json())
      .then((json) => setData(json))
      .catch((err) => console.error("Erro ao buscar dados de corretagem:", err));
  }, []);

  return (
    <div className="p-6 shadow rounded-2xl bg-white">
      <h2 className="text-xl font-bold mb-4">Corretagem Total por Usuário</h2>
      <ResponsiveContainer width="100%" height={350}>
        <BarChart data={data} margin={{ top: 20, right: 30, left: 0, bottom: 5 }}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="usuario" />
          <YAxis />
          <Tooltip />
          <Legend />
          <Bar dataKey="totalCorretagem" name="Corretagem Total (R$)" fill="#8884d8" />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
}