import { useEffect, useState } from "react";
import axios from "axios";

interface ResumoFinanceiro {
  totalInvestido: number;
  carteiraAtual: number;
  lucroPrejuizo: number;
}

interface Props {
  usuarioId: number;
}

export default function ResumoFinanceiroCard({ usuarioId }: Props) {
  const [resumo, setResumo] = useState<ResumoFinanceiro | null>(null);

  useEffect(() => {
    axios
      .get<ResumoFinanceiro>(`https://localhost:5183/api/relatorios/resumo-financeiro/${usuarioId}`)
      .then((response) => setResumo(response.data))
      .catch((error) => console.error("Erro ao buscar resumo financeiro:", error));
  }, [usuarioId]);

  if (!resumo) return <div>Carregando resumo financeiro...</div>;

  return (
    <div className="bg-white p-4 rounded-2xl shadow-md w-full max-w-md">
      <h2 className="text-xl font-bold mb-4">Resumo Financeiro</h2>
      <div className="grid grid-cols-1 gap-2">
        <div>
          <span className="font-semibold">Total Investido: </span>
          R$ {resumo.totalInvestido.toFixed(2)}
        </div>
        <div>
          <span className="font-semibold">Carteira Atual: </span>
          R$ {resumo.carteiraAtual.toFixed(2)}
        </div>
        <div>
          <span className="font-semibold">Lucro / Prejuízo: </span>
          <span className={resumo.lucroPrejuizo >= 0 ? "text-green-600" : "text-red-600"}>
            R$ {resumo.lucroPrejuizo.toFixed(2)}
          </span>
        </div>
      </div>
    </div>
  );
}
