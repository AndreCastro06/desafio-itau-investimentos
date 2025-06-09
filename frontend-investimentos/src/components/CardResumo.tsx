interface Props {
  titulo: string;
  valor: string;
}

function CardResumo({ titulo, valor }: Props) {
  return (
    <div className="bg-white shadow-md rounded-lg p-4 text-center">
      <p className="text-gray-600 text-sm">{titulo}</p>
      <p className="text-xl font-semibold text-gray-900">{valor}</p>
    </div>
  );
}

export default CardResumo;