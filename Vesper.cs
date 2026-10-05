using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

[assembly: AssemblyTitle("Vesper")]
[assembly: AssemblyProduct("Vesper")]
[assembly: AssemblyDescription("Cofre local de arquivos para Windows")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.0.0")]

namespace VesperVault
{
    internal static class Program
    {
        internal const string Version = "V1.0";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string vaultDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Vesper",
                "vault_data");
            Directory.CreateDirectory(vaultDirectory);
            Application.Run(new VaultForm(vaultDirectory));
        }
    }

    internal sealed class VaultForm : Form
    {
        private readonly string vaultDirectory;
        private readonly ListBox filesList;
        private readonly Label statusLabel;

        internal VaultForm(string directory)
        {
            vaultDirectory = directory;
            Text = "Vesper " + Program.Version + " — Cofre arcano";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(620, 520);
            Size = new Size(1040, 700);
            BackColor = Color.FromArgb(15, 23, 42);
            ForeColor = Color.FromArgb(241, 245, 249);
            Font = new Font("Segoe UI", 10F);
            AutoScaleMode = AutoScaleMode.Dpi;

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(16);
            layout.BackColor = BackColor;
            layout.ColumnCount = 2;
            layout.RowCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 69F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Controls.Add(layout);

            Panel mascotPanel = new Panel();
            mascotPanel.Dock = DockStyle.Fill;
            mascotPanel.Margin = new Padding(0, 0, 12, 0);
            mascotPanel.Padding = new Padding(16);
            mascotPanel.BackColor = Color.FromArgb(17, 24, 39);
            mascotPanel.BorderStyle = BorderStyle.FixedSingle;
            layout.Controls.Add(mascotPanel, 0, 0);

            TableLayoutPanel mascotLayout = new TableLayoutPanel();
            mascotLayout.Dock = DockStyle.Fill;
            mascotLayout.ColumnCount = 1;
            mascotLayout.RowCount = 3;
            mascotLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 68F));
            mascotLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            mascotLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));
            mascotPanel.Controls.Add(mascotLayout);

            VesperControl vesper = new VesperControl();
            vesper.Dock = DockStyle.Fill;
            vesper.Margin = new Padding(0);
            mascotLayout.Controls.Add(vesper, 0, 0);

            Label mascotTitle = new Label();
            mascotTitle.Text = "VESPER";
            mascotTitle.Dock = DockStyle.Fill;
            mascotTitle.TextAlign = ContentAlignment.MiddleCenter;
            mascotTitle.ForeColor = Color.FromArgb(196, 181, 253);
            mascotTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            mascotLayout.Controls.Add(mascotTitle, 0, 1);

            Label speechLabel = new Label();
            speechLabel.Text = "Oi. Eu sou Vesper. Vou guardar companhia enquanto o cofre guarda seus arquivos.";
            speechLabel.Dock = DockStyle.Fill;
            speechLabel.Margin = new Padding(4, 8, 4, 8);
            speechLabel.Padding = new Padding(12);
            speechLabel.BackColor = Color.FromArgb(2, 6, 23);
            speechLabel.ForeColor = Color.FromArgb(226, 232, 240);
            speechLabel.BorderStyle = BorderStyle.FixedSingle;
            speechLabel.TextAlign = ContentAlignment.MiddleCenter;
            speechLabel.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
            mascotLayout.Controls.Add(speechLabel, 0, 2);

            string[] sayings = new string[] {
                "A noite é escura porque não há sol. O mistério, às vezes, é só isso.",
                "A porta está fechada porque ainda não foi aberta. A magia começa com observações.",
                "Uma senha esquecida é uma lembrança que escolheu se esconder.",
                "O silêncio não faz barulho. É por isso que o chamamos de silêncio.",
                "Tudo que entra no cofre fica dentro, até alguém abrir o cofre.",
                "A lua aparece à noite porque durante o dia o céu está ocupado.",
                "Se você salvou o arquivo, ele foi salvo. Se cancelou, não foi.",
                "O segredo mais bem guardado é aquele que ninguém tentou abrir ainda.",
                "Uma senha longa pode ser forte; uma senha anotada no lugar errado pode ser famosa.",
                "O cofre protege arquivos, mas não protege você de esquecer a senha.",
                "Cada arquivo tem uma chave própria. Até os segredos gostam de privacidade.",
                "Antes de mover um arquivo, confira se escolheu o arquivo certo. Surpreendente, eu sei.",
                "O botão espera pacientemente. Ele não tem para onde ir.",
                "A criptografia transforma conteúdo legível em conteúdo ilegível sem a chave correta.",
                "O arquivo criptografado não sabe sua senha. Ele só sabe se a chave combina.",
                "Se um arquivo é importante, um backup também é importante. A ironia é muito prática.",
                "O Vesper guarda segredos. Não guarda a senha por você.",
                "Uma cópia de segurança é uma segunda chance disfarçada de arquivo.",
                "O nome do arquivo diz como chamá-lo; não revela o que ele pensa.",
                "O cofre está neste computador. A nuvem, hoje, está dispensada.",
                "Sem internet, a internet não consegue participar desta conversa.",
                "Um arquivo fechado continua existindo. Só ficou menos conversador.",
                "Uma senha boa deve ser difícil de adivinhar e fácil de lembrar para você.",
                "Não use a mesma senha em tudo. Até os mistérios merecem separação.",
                "Se você cancelar a abertura, seu arquivo continua protegido. O cancelamento foi respeitado.",
                "Salvar em uma pasta errada ainda é salvar. Só muda o lugar da surpresa.",
                "A extensão .ves informa o formato, não a qualidade do segredo.",
                "O conteúdo fica cifrado; o tamanho do arquivo ainda pode contar algumas coisas.",
                "Seu computador guarda o cofre. Seu disco também merece cuidados.",
                "A tela está acesa porque há energia. Uma revelação elétrica.",
                "Uma janela é uma janela, mesmo quando mostra um cofre.",
                "O tempo passa um segundo de cada vez, exceto quando esperamos uma barra de progresso.",
                "A gravidade puxa as coisas para baixo. Inclusive minhas expectativas sobre senhas curtas.",
                "Se não encontrar o arquivo, talvez ele esteja numa pasta com outro nome.",
                "O arquivo original só sai depois que a versão protegida foi criada.",
                "Se a criptografia falhar, o original deve permanecer. Segurança também é não perder dados.",
                "A senha não aparece na tela porque segredos não precisam de plateia.",
                "O teclado transforma pensamentos em letras, com resultados variados.",
                "Um clique é só um clique. O que acontece depois é que chama a atenção.",
                "Um backup fora do computador ajuda quando o computador decide ter um dia difícil.",
                "Se o computador quebrar e o backup também, o universo está fazendo uma piada longa.",
                "Um arquivo sem senha não é tímido; só está sem proteção.",
                "A criptografia forte não adivinha senhas fracas. Ela apenas as testa com paciência.",
                "Quanto mais longa a senha, mais possibilidades para quem tenta adivinhar.",
                "Não compartilhe sua senha no mesmo lugar em que compartilha o arquivo.",
                "Uma frase secreta pode ser memorável sem ser uma frase famosa.",
                "O arquivo não pode contar a senha se você não a escrever dentro dele.",
                "Quando o destino já existe, confirme antes de substituir. A prudência tem uma caixa de diálogo.",
                "O botão Abrir abre. O botão Cancelar cancela. A interface gosta de honestidade.",
                "O cofre está vazio até você colocar algo nele. O vazio é muito organizado.",
                "Um arquivo muito importante merece uma pausa antes do clique.",
                "O computador não julga suas escolhas. Eu observo em silêncio, com carinho.",
                "Se a senha está errada, não é pessoal. A matemática também tem limites sociais.",
                "Uma senha correta abre o arquivo; uma senha incorreta abre uma oportunidade de lembrar.",
                "O segredo continua secreto mesmo quando o arquivo tem nome bastante óbvio.",
                "Chamar um arquivo de segredo.txt não o torna mais discreto.",
                "A pasta do cofre é uma pasta. O nome dramático é cortesia minha.",
                "A criptografia fecha a cortina; não apaga o palco.",
                "Mover o original não apaga possíveis cópias em backups ou na nuvem.",
                "Um SSD não esquece necessariamente só porque você pediu com educação.",
                "Privacidade também depende de quem pode usar sua conta no Windows.",
                "Um antivírus atento e atualizações ajudam a proteger o computador inteiro.",
                "O Vesper não acessa a internet. Isso é silêncio digital, não invisibilidade mágica.",
                "Um aplicativo local ainda depende do cuidado com o dispositivo onde está.",
                "Desligar a tela não desliga o mundo. Só reduz a iluminação.",
                "Uma senha guardada na memória é útil; uma memória cansada é imprevisível.",
                "Se precisar anotar a senha, guarde a anotação como se ela também fosse um segredo.",
                "Senhas únicas são como chaves únicas: uma porta, uma chave.",
                "Não envie seu arquivo secreto para alguém só para perguntar se ele é secreto.",
                "Você pode mudar o nome do arquivo. A criptografia não se ofende.",
                "Uma extensão errada não muda o conteúdo, mas pode confundir você no futuro.",
                "A lista mostra arquivos guardados, não mede o tamanho dos seus segredos.",
                "Se a lista não atualizou, talvez seja hora de olhar de novo. A tecnologia também pisca.",
                "O progresso pode demorar em arquivos enormes. Até magia lê byte por byte.",
                "Arquivos grandes pedem paciência e espaço em disco. O cofre não cria espaço do nada.",
                "Antes de apagar qualquer coisa, confirme que a cópia certa está salva.",
                "A senha é sua. O arquivo é seu. A responsabilidade também faz parte do pacote.",
                "O melhor segredo é aquele que você consegue recuperar quando precisa.",
                "A curiosidade abre portas; a senha correta abre este arquivo.",
                "A estrela brilha porque emite luz. Eu brilho porque alguém desenhou assim.",
                "Meus chifrinhos são decorativos. Eles não capturam senhas, nem moscas.",
                "Minha barriga é branca porque alguém escolheu essa cor. Arte é uma decisão.",
                "Eu sou fofo por fora e extremamente sério sobre arquivos por dentro.",
                "Se eu pudesse piscar, faria isso quando você escolhesse uma senha forte.",
                "Não tenho bolsos, então guardo tudo em pastas.",
                "Eu flutuo porque o chão ainda não apresentou um argumento convincente.",
                "Uma estrela no alto da cabeça é um lembrete para olhar para cima de vez em quando.",
                "Sou pequeno na tela, mas meus conselhos sobre backup são enormes.",
                "Não sou um gato. Também não sou uma pasta. Sou Vesper, uma criatura com prioridades.",
                "Às vezes, a melhor magia é uma mensagem clara e um botão no lugar certo.",
                "Se você está lendo isto, então a caixa de fala está funcionando. Que alívio.",
                "A minha voz é texto. Ainda assim, espero que faça boa companhia.",
                "Eu não durmo. Só espero o próximo intervalo de trinta segundos.",
                "Se eu repetir uma fala, considere que foi uma reprise artística.",
                "O universo é vasto. Esta janela, por outro lado, tem bordas bem definidas.",
                "Uma estrela cadente está caindo. Espero que tenha escolhido um bom destino.",
                "A poeira estelar é poeira com uma equipe de relações públicas excelente.",
                "A luz da Lua levou um pouco mais de um segundo para chegar até você.",
                "Algumas estrelas que vemos já mudaram muito; a luz delas ainda está a caminho.",
                "O espaço parece vazio, mas há muita coisa acontecendo que não vemos.",
                "A água parece comum até você notar que seu corpo precisa dela.",
                "O polvo tem três corações. Eu tenho zero, mas continuo muito empático.",
                "As abelhas dançam para indicar onde há comida. Eu falo para indicar onde há arquivos.",
                "Uma girafa também tem sete vértebras no pescoço, como uma pessoa. A natureza gosta de reutilizar peças.",
                "O mel pode durar muito tempo quando armazenado corretamente. Segredos também pedem bons recipientes.",
                "O cheiro de chuva tem nome: petricor. O cheiro de arquivo salvo ainda não tem.",
                "A palavra que você procura pode estar na ponta da língua. A senha, espero, está na memória.",
                "Uma pausa curta pode ajudar a lembrar uma senha. Ou a inventar outra.",
                "Quando o arquivo abre, a criptografia fez seu trabalho e eu posso relaxar.",
                "Quando o arquivo não abre, não tente adivinhar para sempre. Respire e confira com calma.",
                "A segurança é feita de hábitos pequenos repetidos com cuidado.",
                "O melhor botão é aquele que faz exatamente o que promete.",
                "Uma interface bonita também deve ser clara. Mistério só nas minhas falas.",
                "Se a janela se ajusta ao espaço, ninguém precisa se encolher para caber.",
                "A cada trinta segundos, uma nova frase. O relógio e eu fizemos um acordo.",
                "Talvez você esteja ocupado. Eu espero. Sou excelente em esperar.",
                "Um cofre não precisa de pressa. O segredo também não.",
                "Se hoje foi longo, que ao menos sua senha seja fácil de lembrar para você.",
                "O dia termina quando a Terra gira para longe do Sol. Sim, o planeta faz isso diariamente.",
                "O Sol parece nascer, mas é a Terra que está girando. Dramático, mas tecnicamente correto.",
                "A sombra aparece quando algo bloqueia a luz. Um mistério com testemunha.",
                "Uma chave não é uma fechadura. Mesmo assim, uma precisa da outra.",
                "O arquivo está guardado porque foi colocado aqui. Às vezes, a resposta é literal.",
                "Se você precisa de mim, estou na caixa ao lado. Se não precisa, continuo aqui mesmo.",
                "Um segredo compartilhado deixa de ser segredo exclusivo. A aritmética da confiança.",
                "A confiança leva tempo para crescer e pouco tempo para pedir uma senha.",
                "Todo arquivo tem bytes. Alguns bytes são mais reservados que outros.",
                "O zero é uma quantidade. O vazio, por sua vez, está muito satisfeito consigo mesmo.",
                "Uma pasta pode conter arquivos sem saber o que significam. Eu também, às vezes.",
                "O nome Vesper lembra o entardecer. É uma hora bonita para guardar mistérios.",
                "A magia é uma explicação provisória para quem ainda não abriu o manual.",
                "A explicação é simples: o arquivo está criptografado. O drama é opcional.",
                "Não compartilhe uma senha só porque alguém pediu com muitos pontos de exclamação.",
                "A senha correta é aquela que você escolheu. A minha opinião não altera os bytes.",
                "A tela mostra o que o programa sabe. O programa sabe menos que você imagina.",
                "Lembre-se: o cofre protege o arquivo, mas não faz backup sozinho.",
                "Seu arquivo está protegido. Agora posso voltar a contemplar esta estrela."
            };
            Random sayingPicker = new Random();
            int lastSaying = 0;
            Action speak = delegate
            {
                int nextSaying;
                do
                {
                    nextSaying = sayingPicker.Next(sayings.Length);
                }
                while (sayings.Length > 1 && nextSaying == lastSaying);
                lastSaying = nextSaying;
                speechLabel.Text = sayings[nextSaying];
                vesper.Invalidate();
            };
            Timer idleSpeechTimer = new Timer();
            idleSpeechTimer.Interval = 30000;
            idleSpeechTimer.Tick += delegate { speak(); };
            idleSpeechTimer.Start();
            FormClosed += delegate { idleSpeechTimer.Stop(); idleSpeechTimer.Dispose(); };

            Panel content = new Panel();
            content.Dock = DockStyle.Fill;
            content.Margin = new Padding(12, 0, 0, 0);
            content.Padding = new Padding(22, 18, 22, 16);
            content.BackColor = Color.FromArgb(17, 24, 39);
            content.BorderStyle = BorderStyle.FixedSingle;
            layout.Controls.Add(content, 1, 0);

            TableLayoutPanel contentLayout = new TableLayoutPanel();
            contentLayout.Dock = DockStyle.Fill;
            contentLayout.ColumnCount = 1;
            contentLayout.RowCount = 5;
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            content.Controls.Add(contentLayout);

            Label title = new Label();
            title.Text = "O cofre do Vesper";
            title.Dock = DockStyle.Fill;
            title.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.TextAlign = ContentAlignment.MiddleLeft;
            contentLayout.Controls.Add(title, 0, 0);

            Label subtitle = new Label();
            subtitle.Text = "Seus arquivos, criptografados e guardados neste computador.";
            subtitle.Dock = DockStyle.Fill;
            subtitle.ForeColor = Color.FromArgb(203, 213, 225);
            subtitle.TextAlign = ContentAlignment.MiddleLeft;
            contentLayout.Controls.Add(subtitle, 0, 1);

            Button addButton = CreateButton("Criptografar arquivo", Color.FromArgb(124, 58, 237));
            addButton.Dock = DockStyle.Fill;
            addButton.Margin = new Padding(0, 5, 0, 5);
            addButton.Click += AddFile;
            contentLayout.Controls.Add(addButton, 0, 2);

            filesList = new ListBox();
            filesList.Dock = DockStyle.Fill;
            filesList.Margin = new Padding(0, 8, 0, 8);
            filesList.BackColor = Color.FromArgb(2, 6, 23);
            filesList.ForeColor = Color.FromArgb(241, 245, 249);
            filesList.BorderStyle = BorderStyle.FixedSingle;
            filesList.Font = new Font("Consolas", 10F);
            filesList.IntegralHeight = false;
            contentLayout.Controls.Add(filesList, 0, 3);

            TableLayoutPanel footer = new TableLayoutPanel();
            footer.Dock = DockStyle.Fill;
            footer.ColumnCount = 1;
            footer.RowCount = 2;
            footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentLayout.Controls.Add(footer, 0, 4);

            Button openButton = CreateButton("Abrir arquivo selecionado", Color.FromArgb(37, 99, 235));
            openButton.Dock = DockStyle.Fill;
            openButton.Click += OpenSelectedFile;
            footer.Controls.Add(openButton, 0, 0);

            statusLabel = new Label();
            statusLabel.Dock = DockStyle.Fill;
            statusLabel.Margin = new Padding(0, 5, 0, 0);
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            statusLabel.ForeColor = Color.FromArgb(203, 213, 225);
            statusLabel.AutoEllipsis = true;
            footer.Controls.Add(statusLabel, 0, 1);

            Resize += delegate { UpdateResponsiveLayout(layout, mascotPanel, content); };
            UpdateResponsiveLayout(layout, mascotPanel, content);

            RefreshFiles();
            statusLabel.Text = "Armazenamento local: " + vaultDirectory;
        }

        private void UpdateResponsiveLayout(TableLayoutPanel layout, Panel mascotPanel, Panel content)
        {
            bool narrow = ClientSize.Width < 860 && ClientSize.Height >= 700;
            layout.SuspendLayout();
            if (narrow)
            {
                layout.ColumnCount = 1;
                layout.RowCount = 2;
                layout.ColumnStyles.Clear();
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                layout.RowStyles.Clear();
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 36F));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 64F));
                mascotPanel.Margin = new Padding(0, 0, 0, 10);
                content.Margin = new Padding(0, 10, 0, 0);
                layout.SetCellPosition(mascotPanel, new TableLayoutPanelCellPosition(0, 0));
                layout.SetCellPosition(content, new TableLayoutPanelCellPosition(0, 1));
            }
            else
            {
                bool compactWidth = ClientSize.Width < 860;
                layout.ColumnCount = 2;
                layout.RowCount = 1;
                layout.ColumnStyles.Clear();
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, compactWidth ? 25F : 31F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, compactWidth ? 75F : 69F));
                layout.RowStyles.Clear();
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                mascotPanel.Margin = new Padding(0, 0, 12, 0);
                content.Margin = new Padding(12, 0, 0, 0);
                layout.SetCellPosition(mascotPanel, new TableLayoutPanelCellPosition(0, 0));
                layout.SetCellPosition(content, new TableLayoutPanelCellPosition(1, 0));
            }
            layout.ResumeLayout(true);
        }

        private static Button CreateButton(string text, Color color)
        {
            Button button = new Button();
            button.Text = text;
            button.BackColor = color;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            return button;
        }

        private void AddFile(object sender, EventArgs e)
        {
            using (OpenFileDialog picker = new OpenFileDialog())
            {
                picker.Title = "Selecione o arquivo que será criptografado e movido para o cofre";
                picker.CheckFileExists = true;
                picker.Multiselect = false;
                if (picker.ShowDialog(this) != DialogResult.OK)
                {
                    statusLabel.Text = "Seleção cancelada. Nenhum arquivo foi alterado.";
                    return;
                }

                using (PasswordForm passwordDialog = new PasswordForm("Criar senha deste arquivo"))
                {
                    if (passwordDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        statusLabel.Text = "Operação cancelada. O arquivo original continua no lugar.";
                        return;
                    }

                    string encryptedPath = null;
                    try
                    {
                        encryptedPath = VaultCrypto.EncryptFile(picker.FileName, vaultDirectory, passwordDialog.Password);
                        try
                        {
                            File.Delete(picker.FileName);
                            statusLabel.Text = "Arquivo criptografado e movido para o cofre: " + Path.GetFileName(encryptedPath);
                        }
                        catch (Exception deleteError)
                        {
                            statusLabel.Text = "A versão criptografada está no cofre, mas o original sem criptografia permaneceu: " + deleteError.Message;
                            MessageBox.Show(this,
                                "A versão criptografada foi salva no cofre, mas o arquivo original sem criptografia ainda está no local de origem.\n\n" +
                                deleteError.Message + "\n\nA cópia criptografada está em:\n" + encryptedPath,
                                "O arquivo original permaneceu no disco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        RefreshFiles();
                    }
                    catch (Exception error)
                    {
                        statusLabel.Text = "Falha: " + error.Message;
                        MessageBox.Show(this, error.Message, "Não foi possível guardar o arquivo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void OpenSelectedFile(object sender, EventArgs e)
        {
            string filename = filesList.SelectedItem as string;
            if (filename == null || !filename.EndsWith(VaultCrypto.Extension, StringComparison.OrdinalIgnoreCase))
            {
                statusLabel.Text = "Selecione um arquivo .ves do cofre.";
                return;
            }

            using (PasswordForm passwordDialog = new PasswordForm("Senha do arquivo selecionado"))
            {
                if (passwordDialog.ShowDialog(this) != DialogResult.OK)
                {
                    statusLabel.Text = "Abertura cancelada. O arquivo continua no cofre.";
                    return;
                }

                using (SaveFileDialog picker = new SaveFileDialog())
                {
                    picker.Title = "Escolha onde salvar o arquivo aberto";
                    picker.FileName = filename.Substring(0, filename.Length - VaultCrypto.Extension.Length);
                    picker.OverwritePrompt = true;
                    if (picker.ShowDialog(this) != DialogResult.OK)
                    {
                        statusLabel.Text = "Salvamento cancelado. O arquivo criptografado continua no cofre.";
                        return;
                    }

                    string encryptedPath = Path.Combine(vaultDirectory, filename);
                    try
                    {
                        VaultCrypto.DecryptFile(encryptedPath, picker.FileName, passwordDialog.Password);
                        try
                        {
                            File.Delete(encryptedPath);
                            statusLabel.Text = "Salvo. O arquivo criptografado foi retirado do cofre.";
                            RefreshFiles();
                        }
                        catch (Exception deleteError)
                        {
                            statusLabel.Text = "Arquivo salvo; não foi possível remover o arquivo criptografado: " + deleteError.Message;
                            MessageBox.Show(this,
                                "O arquivo foi salvo, mas o arquivo criptografado ainda permanece no cofre.\n\n" + deleteError.Message,
                                "Arquivo criptografado mantido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (CryptographicException)
                    {
                        statusLabel.Text = "Senha incorreta ou arquivo danificado. O arquivo criptografado foi mantido.";
                        MessageBox.Show(this, "Senha incorreta ou arquivo danificado. O arquivo continua no cofre.",
                            "Não foi possível abrir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception error)
                    {
                        statusLabel.Text = "Falha ao salvar. O arquivo criptografado continua no cofre.";
                        MessageBox.Show(this, error.Message, "Não foi possível abrir o arquivo",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshFiles()
        {
            string selected = filesList.SelectedItem as string;
            filesList.BeginUpdate();
            filesList.Items.Clear();
            string[] paths = Directory.GetFiles(vaultDirectory, "*" + VaultCrypto.Extension);
            Array.Sort(paths, StringComparer.OrdinalIgnoreCase);
            if (paths.Length == 0)
                filesList.Items.Add("O cofre está vazio — seus segredos ainda não chegaram.");
            foreach (string path in paths)
            {
                string name = Path.GetFileName(path);
                int index = filesList.Items.Add(name);
                if (String.Equals(name, selected, StringComparison.OrdinalIgnoreCase))
                {
                    filesList.SelectedIndex = index;
                }
            }
            filesList.EndUpdate();
        }
    }

    internal sealed class PasswordForm : Form
    {
        private readonly TextBox passwordBox;
        internal string Password { get { return passwordBox.Text; } }

        internal PasswordForm(string prompt)
        {
            Text = prompt;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(390, 160);
            BackColor = Color.FromArgb(17, 24, 39);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10F);

            Label label = new Label();
            label.Text = prompt;
            label.SetBounds(18, 12, 350, 24);
            Controls.Add(label);

            passwordBox = new TextBox();
            passwordBox.UseSystemPasswordChar = true;
            passwordBox.SetBounds(18, 42, 350, 28);
            passwordBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(passwordBox);

            Label hint = new Label();
            hint.Text = "Sem limite mínimo. Senhas curtas são mais fáceis de adivinhar.";
            hint.SetBounds(18, 75, 350, 32);
            hint.ForeColor = Color.FromArgb(203, 213, 225);
            Controls.Add(hint);

            Button ok = new Button();
            ok.Text = "Continuar";
            ok.DialogResult = DialogResult.OK;
            ok.SetBounds(184, 116, 88, 32);
            Controls.Add(ok);

            Button cancel = new Button();
            cancel.Text = "Cancelar";
            cancel.DialogResult = DialogResult.Cancel;
            cancel.SetBounds(280, 116, 88, 32);
            Controls.Add(cancel);
            AcceptButton = ok;
            CancelButton = cancel;
        }
    }

    internal sealed class VesperControl : Control
    {
        internal VesperControl()
        {
            DoubleBuffered = true;
            BackColor = Color.FromArgb(17, 24, 39);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float scale = Math.Min(ClientSize.Width / 220F, ClientSize.Height / 220F);
            float ox = (ClientSize.Width - 220F * scale) / 2F;
            float oy = (ClientSize.Height - 220F * scale) / 2F;
            GraphicsState state = g.Save();
            g.TranslateTransform(ox, oy);
            g.ScaleTransform(scale, scale);

            using (SolidBrush aura = new SolidBrush(Color.FromArgb(35, 196, 181, 253)))
                g.FillEllipse(aura, 28, 25, 164, 164);
            using (SolidBrush horn = new SolidBrush(Color.FromArgb(255, 224, 164)))
            {
                g.FillEllipse(horn, 54, 43, 38, 42);
                g.FillEllipse(horn, 128, 43, 38, 42);
            }
            using (SolidBrush body = new SolidBrush(Color.FromArgb(177, 151, 231)))
                g.FillEllipse(body, 43, 58, 134, 139);
            using (SolidBrush paw = new SolidBrush(Color.FromArgb(145, 119, 207)))
            {
                g.FillEllipse(paw, 34, 128, 38, 27);
                g.FillEllipse(paw, 148, 128, 38, 27);
                g.FillEllipse(paw, 66, 175, 33, 20);
                g.FillEllipse(paw, 121, 175, 33, 20);
            }
            using (SolidBrush belly = new SolidBrush(Color.FromArgb(243, 237, 255)))
                g.FillEllipse(belly, 76, 137, 68, 43);
            using (SolidBrush eye = new SolidBrush(Color.FromArgb(50, 38, 90)))
            {
                g.FillEllipse(eye, 78, 104, 12, 18);
                g.FillEllipse(eye, 130, 104, 12, 18);
            }
            using (SolidBrush shine = new SolidBrush(Color.White))
            {
                g.FillEllipse(shine, 81, 106, 4, 4);
                g.FillEllipse(shine, 133, 106, 4, 4);
            }
            using (SolidBrush blush = new SolidBrush(Color.FromArgb(220, 237, 158, 190)))
            {
                g.FillEllipse(blush, 64, 125, 17, 8);
                g.FillEllipse(blush, 139, 125, 17, 8);
            }
            using (Pen smile = new Pen(Color.FromArgb(81, 58, 124), 3F))
            {
                g.DrawArc(smile, 96, 124, 14, 12, 20, 140);
                g.DrawArc(smile, 110, 124, 14, 12, 20, 140);
            }
            using (SolidBrush star = new SolidBrush(Color.FromArgb(255, 231, 168)))
            {
                PointF[] points = new PointF[] {
                    new PointF(110, 22), new PointF(114, 36), new PointF(128, 40),
                    new PointF(114, 44), new PointF(110, 58), new PointF(106, 44),
                    new PointF(92, 40), new PointF(106, 36)
                };
                g.FillPolygon(star, points);
            }
            g.Restore(state);
        }
    }

    internal static class VaultCrypto
    {
        internal const string Extension = ".ves";
        private static readonly byte[] Magic = Encoding.ASCII.GetBytes("ESYSAFE2");
        private const int SaltLength = 16;
        private const int IvLength = 16;
        private const int TagLength = 32;
        private const int Iterations = 600000;
        private const int HeaderLength = 8 + 4 + SaltLength + IvLength;
        private const int BufferSize = 64 * 1024;

        internal static string EncryptFile(string sourcePath, string vaultDirectory, string password)
        {
            string baseName = Path.GetFileName(sourcePath);
            string destination = UniquePath(Path.Combine(vaultDirectory, baseName + Extension));
            string temporary = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                EncryptToFile(sourcePath, temporary, password);
                File.Move(temporary, destination);
                return destination;
            }
            finally
            {
                if (File.Exists(temporary))
                    File.Delete(temporary);
            }
        }

        internal static void DecryptFile(string encryptedPath, string destinationPath, string password)
        {
            string fullEncryptedPath = Path.GetFullPath(encryptedPath);
            string fullDestinationPath = Path.GetFullPath(destinationPath);
            if (String.Equals(fullEncryptedPath, fullDestinationPath, StringComparison.OrdinalIgnoreCase))
                throw new IOException("Escolha um destino diferente do arquivo criptografado.");

            string temporaryPath = fullDestinationPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                DecryptToFile(encryptedPath, temporaryPath, password);
                if (File.Exists(fullDestinationPath))
                    File.Replace(temporaryPath, fullDestinationPath, null);
                else
                    File.Move(temporaryPath, fullDestinationPath);
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        private static void DecryptToFile(string encryptedPath, string destinationPath, string password)
        {
            using (FileStream input = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] header = ReadExact(input, HeaderLength);
                byte[] salt = new byte[SaltLength];
                byte[] iv = new byte[IvLength];
                Buffer.BlockCopy(header, 12, salt, 0, SaltLength);
                Buffer.BlockCopy(header, 12 + SaltLength, iv, 0, IvLength);
                int iterations = BitConverter.ToInt32(header, 8);
                ValidateHeader(header, iterations);

                long cipherLength = input.Length - HeaderLength - TagLength;
                if (cipherLength <= 0 || cipherLength % 16 != 0)
                    throw new CryptographicException("Formato do arquivo inválido.");

                byte[] keys = DeriveKeys(password, salt, iterations);
                try
                {
                    byte[] expectedTag = new byte[TagLength];
                    input.Position = HeaderLength + cipherLength;
                    ReadExact(input, expectedTag, 0, TagLength);
                    input.Position = 0;
                    byte[] actualTag = ComputeTag(keys, input, HeaderLength + cipherLength);
                    if (!ConstantTimeEquals(expectedTag, actualTag))
                        throw new CryptographicException("Senha incorreta ou arquivo danificado.");

                    input.Position = HeaderLength;
                    using (Aes aes = Aes.Create())
                    {
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;
                        aes.Key = CopyRange(keys, 0, 32);
                        aes.IV = iv;

                        using (LimitedReadStream limited = new LimitedReadStream(input, cipherLength))
                        using (CryptoStream crypto = new CryptoStream(limited, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        using (FileStream output = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            crypto.CopyTo(output, BufferSize);
                            output.Flush(true);
                        }
                    }
                }
                finally
                {
                    Array.Clear(keys, 0, keys.Length);
                }
            }
        }

        private static void EncryptToFile(string sourcePath, string destinationPath, string password)
        {
            byte[] salt = new byte[SaltLength];
            byte[] iv = new byte[IvLength];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                rng.GetBytes(iv);
            }
            byte[] header = new byte[HeaderLength];
            Buffer.BlockCopy(Magic, 0, header, 0, Magic.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(Iterations), 0, header, 8, 4);
            Buffer.BlockCopy(salt, 0, header, 12, SaltLength);
            Buffer.BlockCopy(iv, 0, header, 12 + SaltLength, IvLength);
            byte[] keys = DeriveKeys(password, salt, Iterations);

            try
            {
                using (Aes aes = Aes.Create())
                using (HMACSHA256 hmac = new HMACSHA256(CopyRange(keys, 32, 32)))
                using (FileStream output = new FileStream(destinationPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (FileStream input = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = CopyRange(keys, 0, 32);
                    aes.IV = iv;
                    output.Write(header, 0, header.Length);
                    hmac.TransformBlock(header, 0, header.Length, header, 0);

                    using (HmacWriteStream authenticatedOutput = new HmacWriteStream(output, hmac))
                    using (CryptoStream crypto = new CryptoStream(authenticatedOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        input.CopyTo(crypto, BufferSize);
                        crypto.FlushFinalBlock();
                    }

                    hmac.TransformFinalBlock(new byte[0], 0, 0);
                    output.Write(hmac.Hash, 0, hmac.Hash.Length);
                    output.Flush(true);
                }
            }
            finally
            {
                Array.Clear(keys, 0, keys.Length);
            }
        }

        private static byte[] DeriveKeys(string password, byte[] salt, int iterations)
        {
            if (iterations < 600000 || iterations > 2000000)
                throw new CryptographicException("Parâmetros de derivação inválidos.");

            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(
                password, salt, iterations, HashAlgorithmName.SHA256))
                return derive.GetBytes(64);
        }

        private static byte[] ComputeTag(byte[] keys, Stream input, long bytesToHash)
        {
            using (HMACSHA256 hmac = new HMACSHA256(CopyRange(keys, 32, 32)))
            {
                byte[] buffer = new byte[BufferSize];
                long remaining = bytesToHash;
                while (remaining > 0)
                {
                    int read = input.Read(buffer, 0, (int)Math.Min(buffer.Length, remaining));
                    if (read <= 0)
                        throw new EndOfStreamException("Arquivo criptografado truncado.");
                    hmac.TransformBlock(buffer, 0, read, buffer, 0);
                    remaining -= read;
                }
                hmac.TransformFinalBlock(new byte[0], 0, 0);
                return hmac.Hash;
            }
        }

        private static void ValidateHeader(byte[] header, int iterations)
        {
            for (int i = 0; i < Magic.Length; i++)
                if (header[i] != Magic[i])
                    throw new CryptographicException("Formato de arquivo não reconhecido.");
            if (iterations < 600000 || iterations > 2000000)
                throw new CryptographicException("Parâmetros de derivação inválidos.");
        }

        private static byte[] ReadExact(Stream stream, int count)
        {
            byte[] result = new byte[count];
            ReadExact(stream, result, 0, count);
            return result;
        }

        private static void ReadExact(Stream stream, byte[] buffer, int offset, int count)
        {
            int total = 0;
            while (total < count)
            {
                int read = stream.Read(buffer, offset + total, count - total);
                if (read <= 0)
                    throw new EndOfStreamException("Arquivo criptografado truncado.");
                total += read;
            }
        }

        private static byte[] CopyRange(byte[] source, int offset, int count)
        {
            byte[] result = new byte[count];
            Buffer.BlockCopy(source, offset, result, 0, count);
            return result;
        }

        private static bool ConstantTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
                return false;
            int difference = 0;
            for (int i = 0; i < left.Length; i++)
                difference |= left[i] ^ right[i];
            return difference == 0;
        }

        private static string UniquePath(string path)
        {
            if (!File.Exists(path))
                return path;
            string directory = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            for (int index = 1; ; index++)
            {
                string candidate = Path.Combine(directory, name + " (" + index + ")" + extension);
                if (!File.Exists(candidate))
                    return candidate;
            }
        }
    }

    internal sealed class HmacWriteStream : Stream
    {
        private readonly Stream output;
        private readonly HMAC hmac;

        internal HmacWriteStream(Stream destination, HMAC hasher)
        {
            output = destination;
            hmac = hasher;
        }

        public override bool CanRead { get { return false; } }
        public override bool CanSeek { get { return false; } }
        public override bool CanWrite { get { return true; } }
        public override long Length { get { throw new NotSupportedException(); } }
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        public override void Flush() { output.Flush(); }
        public override int Read(byte[] buffer, int offset, int count) { throw new NotSupportedException(); }
        public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException(); }
        public override void SetLength(long value) { throw new NotSupportedException(); }

        public override void Write(byte[] buffer, int offset, int count)
        {
            hmac.TransformBlock(buffer, offset, count, buffer, offset);
            output.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                output.Flush();
            base.Dispose(disposing);
        }
    }

    internal sealed class LimitedReadStream : Stream
    {
        private readonly Stream input;
        private long remaining;

        internal LimitedReadStream(Stream source, long length)
        {
            input = source;
            remaining = length;
        }

        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return false; } }
        public override bool CanWrite { get { return false; } }
        public override long Length { get { throw new NotSupportedException(); } }
        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException(); }
        public override void SetLength(long value) { throw new NotSupportedException(); }
        public override void Write(byte[] buffer, int offset, int count) { throw new NotSupportedException(); }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (remaining <= 0)
                return 0;
            int read = input.Read(buffer, offset, (int)Math.Min(count, remaining));
            remaining -= read;
            return read;
        }
    }
}
