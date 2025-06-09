import { useEffect, useState } from "react";
import CardResumo from "./components/CardResumo";
import TabelaPosicoes from "./components/TabelaPosicoes";
import axios from "axios";

function App() {
  const [usuarios, setUsuarios] = useState<{ id: number; nome: string }[]>([]);
  const [usuarioId, setUsuarioId] = useState(0);

  const [totalInvestido, setTotalInvestido] = useState("R$ 0,00");
  const [corretagemTotal, setCorretagemTotal] = useState("R$ 0,00");
  const [carteiraAtual, setCarteiraAtual] = useState("R$ 0,00");
  const [lucroPrejuizo, setLucroPrejuizo] = useState("R$ 0,00");

  useEffect(() => {
    axios.get("https://localhost:5183/api/usuarios")
      .then(res => setUsuarios(res.data));
  }, []);

  useEffect(() => {
    if (!usuarioId) return;

    fetch(`https://localhost:5183/api/relatorios/posicoes-usuario/${usuarioId}`)
      .then(res => res.json())
      .then(data => {
        const totalInvest = data.reduce((acc: number, cur: any) => acc + cur.precoMedio * cur.quantidade, 0);
        const carteira = data.reduce((acc: number, cur: any) => acc + cur.precoAtual * cur.quantidade, 0);
        const lucro = carteira - totalInvest;

        setTotalInvestido(totalInvest.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
        setCarteiraAtual(carteira.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
        setLucroPrejuizo(`${lucro >= 0 ? "+" : "-"}${Math.abs(lucro).toLocaleString("pt-BR", {
          style: "currency",
          currency: "BRL",
        })}`);
      });

    fetch(`https://localhost:5183/api/relatorios/corretagem-total`)
      .then(res => res.json())
      .then(data => {
        const total = data
          .filter((x: any) => x.usuarioId === usuarioId)
          .reduce((acc: number, cur: any) => acc + cur.totalCorretagem, 0);
        setCorretagemTotal(total.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
      });
  }, [usuarioId]);

return (
  <div className="app-container">
    <h1 className="dashboard-header">Painel de Investimentos</h1>

    <div className="select-container">
      <select
        value={usuarioId}
        onChange={(e) => setUsuarioId(Number(e.target.value))}
        className="select-usuario"
      >
        <option value={0}>Selecione um cliente</option>
        {usuarios.map((u) => (
          <option key={u.id} value={u.id}>{u.nome}</option>
        ))}
      </select>
    </div>

    {usuarioId !== 0 && (
      <>
        <div className="cards-grid">
          <CardResumo titulo="Total Investido" valor={totalInvestido} />
          <CardResumo titulo="Corretagem Total" valor={corretagemTotal} />
          <CardResumo titulo="Lucro/Prejuízo" valor={lucroPrejuizo} />
          <CardResumo titulo="Carteira Atual" valor={carteiraAtual} />
        </div>

        <TabelaPosicoes usuarioId={usuarioId} />
      </>
    )}
  </div>
);
}

export default App;