import { useEffect, useState } from "react";
import axios from "axios";
import {
  BarChart, Bar, XAxis, YAxis, Tooltip, PieChart, Pie, Cell, Legend, ResponsiveContainer,
} from "recharts";

// Tipos explícitos
interface Posicao {
  nomeUsuario: string;
  valorTotal: number;
}

interface Corretagem {
  nomeUsuario: string;
  valorTotalCorretagem: number;
}

export default function Relatorios() {
  const [topPosicoes, setTopPosicoes] = useState<Posicao[]>([]);
  const [topCorretagens, setTopCorretagens] = useState<Corretagem[]>([]);

  useEffect(() => {
    async function fetchData() {
      const posicoes = await axios.get("https://localhost:5183/api/relatorios/top-posicoes");
      const corretagens = await axios.get("https://localhost:5183/api/relatorios/corretagem-total");

      // Normaliza os nomes dos campos se necessário
      const corretagensNormalizadas: Corretagem[] = corretagens.data.map((item: any) => ({
        nomeUsuario: item.usuario ?? item.nomeUsuario,
        valorTotalCorretagem: item.totalCorretagem ?? item.ValorTotalCorretagem ?? item.valorTotalCorretagem,
      }));

      const posicoesNormalizadas: Posicao[] = posicoes.data.map((item: any) => ({
        nomeUsuario: item.usuario ?? item.nomeUsuario,
        valorTotal: item.valorTotal ?? item.ValorTotalPL ?? item.valorTotal,
      }));

      setTopPosicoes(posicoesNormalizadas);
      setTopCorretagens(corretagensNormalizadas);
    }

    fetchData();
  }, []);

  return (
    <div className="p-6 space-y-10">
      <h1 className="text-2xl font-bold">Top 10 Clientes por Posição</h1>
      <ResponsiveContainer width="100%" height={400}>
        <BarChart data={topPosicoes}>
          <XAxis dataKey="nomeUsuario" />
          <YAxis />
          <Tooltip />
          <Bar dataKey="valorTotal" fill="#8884d8" />
        </BarChart>
      </ResponsiveContainer>

      <h1 className="text-2xl font-bold">Top 10 Clientes por Corretagem</h1>
      <ResponsiveContainer width="100%" height={400}>
        <PieChart>
          <Pie
            data={topCorretagens}
            dataKey="valorTotalCorretagem"
            nameKey="nomeUsuario"
            cx="50%"
            cy="50%"
            outerRadius={120}
            label
          >
            {topCorretagens.map((entry, index) => (
              <Cell
                key={`cell-${index}`}
                fill={entry.valorTotalCorretagem > 5000 ? "#10b981" : "#ef4444"} // verde ou vermelho
              />
            ))}
          </Pie>
          <Tooltip />
          <Legend />
        </PieChart>
      </ResponsiveContainer>
    </div>
  );
}