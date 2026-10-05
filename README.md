# Vesper V1.0

Vesper é um cofre local de arquivos para Windows. Ele criptografa cada arquivo com uma senha própria e guarda o resultado no cofre com a extensão `.ves`.

## Recursos

- Interface desktop nativa para Windows.
- Criptografia local, sem conexão com a internet ou pacotes de terceiros.
- Senha independente para cada arquivo.
- O arquivo original só é removido depois que o arquivo criptografado foi criado com sucesso.

## Requisitos

- Windows.
- .NET Framework 4.7.2 ou mais recente.

## Compilar e iniciar

1. Baixe ou clone este repositório.
2. No Windows, execute `build.bat`.
3. Abra o `Vesper.exe` gerado.

O script usa o compilador do .NET Framework já instalado e não baixa nem instala pacotes.

## Como usar

1. Para proteger um arquivo, clique em **Criptografar arquivo**, escolha o arquivo e defina uma senha.
2. Para recuperar um arquivo, selecione o `.ves` no cofre, clique em **Abrir arquivo selecionado**, informe a senha correspondente e escolha onde salvar.

O cofre fica em `%LOCALAPPDATA%\Vesper\vault_data`. Se a criação do arquivo protegido falhar, o original não é removido. Se o Windows impedir a remoção do original, o Vesper avisa e mantém ambos. Ao recuperar um arquivo, cancelamentos ou erros mantêm o `.ves`; após salvar com sucesso, ele é removido do cofre.

## Recomendações

- Guarde a senha: ela não pode ser recuperada e cada arquivo tem sua própria senha.
- Use senhas fortes e difíceis de adivinhar. Senhas curtas ou vazias oferecem pouca proteção.
- Mantenha uma cópia de segurança do `.ves` em um local seguro.
- A remoção do original usa a exclusão normal do Windows, não a Lixeira, e não garante que os dados sejam irrecuperáveis.
- O Vesper não passou por auditoria de segurança independente; não existe garantia de segurança absoluta.
