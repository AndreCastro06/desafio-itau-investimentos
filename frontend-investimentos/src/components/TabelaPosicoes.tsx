import { useState, useEffect } from "react";

interface Posicao {
  ativo: string;
  quantidade: number;
  precoMedio: number;
  precoAtual: number;
  pl: number;
}

type Props = {
  usuarioId: number;
};

function TabelaPosicoes({ usuarioId }: Props) {
  const [dados, setDados] = useState<Posicao[]>([]);

  useEffect(() => {
    fetch(`https://localhost:5183/api/relatorios/posicoes-usuario/${usuarioId}`)
      .then(res => res.json())
      .then(data => setDados(data))
      .catch(() => setDados([]));
  }, [usuarioId]);

  return (
    <div className="bg-white shadow-md rounded-lg p-4">
      <h2 className="text-lg font-semibold mb-4">Posições por Ativo</h2>
      <table className="min-w-full text-sm text-left">
        <thead className="text-gray-500 border-b">
          <tr>
            <th className="py-2">Ativo</th>
            <th className="py-2">Quantidade</th>
            <th className="py-2">Preço Médio</th>
            <th className="py-2">Preço Atual</th>
            <th className="py-2">P&L</th>
          </tr>
        </thead>
        <tbody>
          {dados.map((dado, idx) => {
            const cor = dado.pl >= 0 ? "text-green-600" : "text-red-600";
            const prefix = dado.pl >= 0 ? "+" : "-";
            return (
              <tr key={idx} className="border-t">
                <td className="py-2">{dado.ativo}</td>
                <td className="py-2">{dado.quantidade}</td>
                <td className="py-2">R$ {dado.precoMedio.toFixed(2)}</td>
                <td className="py-2">R$ {dado.precoAtual.toFixed(2)}</td>
                <td className={`py-2 font-semibold ${cor}`}>
                  {prefix}R$ {Math.abs(dado.pl).toFixed(2)}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}

export default TabelaPosicoes;