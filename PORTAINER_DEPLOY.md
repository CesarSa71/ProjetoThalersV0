# Guia de Deploy no Portainer.io

Este guia explica como publicar os containers da aplicação no Portainer.io na nuvem.

## 📋 Pré-requisitos

1. Acesso a um servidor com Portainer.io instalado
2. Acesso SSH ao servidor (ou interface web do Portainer)
3. Código-fonte da aplicação disponível (Git ou arquivo compactado)

## 🚀 Opções de Deploy

### Opção 1: Deploy via Git (Recomendado)

#### Passo 1: Preparar o Repositório Git

1. Crie um repositório Git (GitHub, GitLab, Bitbucket, etc.)
2. Faça commit de todos os arquivos do projeto:
   ```bash
   git add .
   git commit -m "Preparar para deploy no Portainer"
   git push origin main
   ```

#### Passo 2: Configurar no Portainer

1. **Acesse o Portainer**
   - Abra o navegador e acesse a URL do Portainer
   - Faça login com suas credenciais

2. **Criar um Stack**
   - No menu lateral, clique em **"Stacks"**
   - Clique em **"Add stack"**
   - Dê um nome para o stack (ex: `projeto-thalers`)

3. **Configurar o Build**
   - Selecione **"Build method"** → **"Repository"**
   - **Repository URL**: Cole a URL do seu repositório Git
   - **Repository reference**: `main` ou `master` (sua branch principal)
   - **Compose path**: `docker-compose.production.yml` (ou `docker-compose.yml`)

4. **Configurar Variáveis de Ambiente (Opcional)**
   - Role até **"Environment variables"**
   - Adicione as variáveis se necessário:
     ```
     POSTGRES_USER=seu_usuario
     POSTGRES_PASSWORD=sua_senha_segura
     POSTGRES_DB=ThalersDb
     APP_PORT=8080
     ADMINER_PORT=8090
     ```

5. **Deploy**
   - Clique em **"Deploy the stack"**
   - Aguarde o build e deploy dos containers

---

### Opção 2: Deploy via Upload de Arquivos

#### Passo 1: Preparar os Arquivos

1. Crie um arquivo compactado (ZIP) com todo o projeto:
   ```bash
   # No diretório do projeto
   zip -r projeto-thalers.zip . -x "*.git*" -x "*bin/*" -x "*obj/*"
   ```

#### Passo 2: Configurar no Portainer

1. **Acesse o Portainer**
   - Abra o navegador e acesse a URL do Portainer
   - Faça login com suas credenciais

2. **Criar um Stack**
   - No menu lateral, clique em **"Stacks"**
   - Clique em **"Add stack"**
   - Dê um nome para o stack (ex: `projeto-thalers`)

3. **Configurar o Build**
   - Selecione **"Build method"** → **"Upload"**
   - Clique em **"Select a file"** e escolha o arquivo ZIP
   - **Compose path**: `docker-compose.production.yml` (ou `docker-compose.yml`)

4. **Configurar Variáveis de Ambiente (Opcional)**
   - Adicione as variáveis se necessário

5. **Deploy**
   - Clique em **"Deploy the stack"**
   - Aguarde o build e deploy dos containers

---

### Opção 3: Deploy via Editor Web

#### Passo 1: Copiar o docker-compose.yml

1. Abra o arquivo `docker-compose.production.yml` no editor
2. Copie todo o conteúdo

#### Passo 2: Configurar no Portainer

1. **Acesse o Portainer**
   - Abra o navegador e acesse a URL do Portainer
   - Faça login com suas credenciais

2. **Criar um Stack**
   - No menu lateral, clique em **"Stacks"**
   - Clique em **"Add stack"**
   - Dê um nome para o stack (ex: `projeto-thalers`)

3. **Configurar o Build**
   - Selecione **"Build method"** → **"Web editor"**
   - Cole o conteúdo do `docker-compose.production.yml` no editor
   - **⚠️ IMPORTANTE**: Para o build funcionar, você precisará fazer upload dos arquivos do projeto separadamente ou usar Git

4. **Configurar Build Context (se necessário)**
   - Se usar o editor web, você precisará fazer upload dos arquivos do projeto
   - Use a opção de **"Upload files"** para enviar o código-fonte

5. **Deploy**
   - Clique em **"Deploy the stack"**

---

## 🔧 Configurações Importantes

### Variáveis de Ambiente Recomendadas

Para produção, configure estas variáveis no Portainer:

```env
POSTGRES_USER=postgres
POSTGRES_PASSWORD=senha_super_segura_aqui
POSTGRES_DB=ThalersDb
POSTGRES_PORT=5432
APP_PORT=8080
ADMINER_PORT=8090
ASPNETCORE_ENVIRONMENT=Production
```

### Portas e Firewall

- **Aplicação .NET**: Porta 8080 (ou a porta configurada em `APP_PORT`)
- **Adminer**: Porta 8090 (ou a porta configurada em `ADMINER_PORT`)
- **PostgreSQL**: Porta 5432 (recomendado manter apenas interno, não expor publicamente)

### Segurança

1. **Senhas**: Use senhas fortes para o PostgreSQL
2. **Adminer**: Considere remover o Adminer em produção ou protegê-lo com autenticação
3. **Firewall**: Configure o firewall para permitir apenas as portas necessárias
4. **HTTPS**: Configure um reverse proxy (Nginx/Traefik) para HTTPS

---

## 📦 Estrutura de Arquivos Necessários

Certifique-se de que estes arquivos estão no repositório/upload:

```
Projeto1/
├── Dockerfile
├── docker-compose.production.yml (ou docker-compose.yml)
├── .dockerignore
├── Projeto1.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Docker.json
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   └── Pessoa.cs
├── Pages/
│   └── (todos os arquivos .razor)
├── Services/
│   └── (todos os serviços)
└── (outros arquivos do projeto)
```

---

## 🔍 Verificação Pós-Deploy

1. **Verificar Containers**
   - No Portainer, vá em **"Containers"**
   - Verifique se todos os 3 containers estão rodando:
     - `projeto1-app`
     - `postgres-thalers`
     - `adminer-thalers`

2. **Verificar Logs**
   - Clique em cada container → **"Logs"**
   - Verifique se não há erros

3. **Testar Aplicação**
   - Acesse: `http://seu-servidor:8080`
   - Verifique se a aplicação carrega corretamente

4. **Testar Adminer (se necessário)**
   - Acesse: `http://seu-servidor:8090`
   - Conecte ao banco usando as credenciais configuradas

---

## 🛠️ Troubleshooting

### Erro: "Cannot connect to database"
- Verifique se o container `postgres-thalers` está rodando
- Verifique as variáveis de ambiente de conexão
- Verifique os logs do PostgreSQL

### Erro: "Build failed"
- Verifique se todos os arquivos necessários estão no repositório
- Verifique se o Dockerfile está correto
- Verifique os logs de build no Portainer

### Erro: "Port already in use"
- Altere as portas nas variáveis de ambiente
- Ou pare os containers que estão usando as portas

### Aplicação não acessível
- Verifique o firewall do servidor
- Verifique se a porta está exposta corretamente
- Verifique os logs do container `projeto1-app`

---

## 📝 Notas Adicionais

1. **Volumes Persistentes**: O PostgreSQL usa um volume persistente (`postgres-data`) para manter os dados mesmo após reiniciar os containers.

2. **Rede Interna**: Todos os containers estão na mesma rede Docker (`app-network`), permitindo comunicação interna.

3. **Health Checks**: O PostgreSQL tem health check configurado, garantindo que a aplicação só inicie quando o banco estiver pronto.

4. **Ambiente de Produção**: O `docker-compose.production.yml` está configurado para `ASPNETCORE_ENVIRONMENT=Production`.

---

## 🔄 Atualizações Futuras

Para atualizar a aplicação:

1. **Via Git**: Faça push das alterações e clique em **"Update the stack"** no Portainer
2. **Via Upload**: Faça upload do novo arquivo ZIP e atualize o stack
3. **Via Editor**: Edite o docker-compose.yml e atualize o stack

O Portainer irá reconstruir apenas os containers que mudaram.

---

## 📞 Suporte

Se encontrar problemas, verifique:
- Logs dos containers no Portainer
- Status dos containers
- Configurações de rede e firewall
- Variáveis de ambiente

