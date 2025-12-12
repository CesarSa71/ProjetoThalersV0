# Configuração HTTPS para Produção

## ⚠️ Diferenças entre Desenvolvimento e Produção

### Desenvolvimento
- ✅ Usa certificados auto-assinados (`dotnet dev-certs`)
- ✅ Apenas para testes locais
- ✅ Navegadores mostram avisos de segurança (aceitável em dev)

### Produção
- ❌ **NÃO** use certificados auto-assinados
- ✅ Use certificados válidos de uma CA confiável
- ✅ Sem avisos de segurança nos navegadores
- ✅ Necessário para confiança dos usuários

## Opções para Certificados em Produção

### 1. Let's Encrypt (Recomendado - Gratuito)

#### Opção A: Usando Certbot
```bash
# Instalar certbot
sudo apt-get install certbot

# Gerar certificado
sudo certbot certonly --standalone -d seu-dominio.com

# Converter para .pfx
openssl pkcs12 -export -out aspnetapp.pfx \
  -inkey /etc/letsencrypt/live/seu-dominio.com/privkey.pem \
  -in /etc/letsencrypt/live/seu-dominio.com/fullchain.pem \
  -password pass:sua_senha
```

#### Opção B: Usando Reverse Proxy (Nginx/Traefik)
- O reverse proxy gerencia SSL/TLS
- A aplicação .NET pode rodar apenas em HTTP internamente
- Mais fácil de gerenciar e renovar certificados

### 2. Certificado do Provedor de Hospedagem
- AWS, Azure, Google Cloud fornecem certificados gerenciados
- Geralmente integrados com Load Balancers

### 3. Certificado Comercial
- Compre de uma CA confiável (DigiCert, GlobalSign, etc.)
- Mais caro, mas com suporte e garantias

## Configuração no Docker Compose

### Variáveis de Ambiente Necessárias

```bash
# Senha do certificado (se aplicável)
CERTIFICATE_PASSWORD=sua_senha_segura

# Portas
APP_PORT=8080
APP_HTTPS_PORT=8081

# Database
POSTGRES_USER=postgres
POSTGRES_PASSWORD=senha_segura
POSTGRES_DB=ThalersDb
```

### Estrutura de Diretórios

```
Projeto1/
├── certs/
│   └── aspnetapp.pfx    # Seu certificado de produção
├── docker-compose.production.yml
└── ...
```

### Executar em Produção

```bash
# 1. Colocar certificado em ./certs/aspnetapp.pfx
# 2. Configurar variáveis de ambiente
export CERTIFICATE_PASSWORD='sua_senha'

# 3. Executar
docker-compose -f docker-compose.production.yml up -d
```

## Usando Reverse Proxy (Recomendado para Produção)

### Exemplo com Nginx

```yaml
# docker-compose.production.yml
services:
  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf:ro
      - ./certs:/etc/nginx/certs:ro
    depends_on:
      - app
  
  app:
    # Remove exposição de portas HTTPS externas
    # Apenas HTTP internamente
    environment:
      - ASPNETCORE_URLS=http://0.0.0.0:8080
    # Não precisa de certificado no .NET
```

### Vantagens do Reverse Proxy
- ✅ Renovação automática de certificados Let's Encrypt
- ✅ Melhor performance (terminação SSL no proxy)
- ✅ Mais fácil de gerenciar múltiplos serviços
- ✅ Rate limiting e outras funcionalidades

## Checklist de Produção

- [ ] Certificado válido de CA confiável
- [ ] Senha do certificado configurada (se aplicável)
- [ ] Variáveis de ambiente seguras (não hardcoded)
- [ ] Portas configuradas corretamente
- [ ] Firewall configurado
- [ ] Renovação automática de certificados (se Let's Encrypt)
- [ ] Monitoramento e logs configurados
- [ ] Backup do certificado

## Segurança Adicional

1. **Use variáveis de ambiente** para senhas:
   ```bash
   # Não faça isso:
   CERTIFICATE_PASSWORD=senha123
   
   # Faça isso:
   export CERTIFICATE_PASSWORD=$(cat /path/to/secret/file)
   ```

2. **Permissões de arquivo**:
   ```bash
   chmod 600 ./certs/aspnetapp.pfx
   ```

3. **Renovação automática** (Let's Encrypt expira a cada 90 dias):
   ```bash
   # Adicionar ao crontab
   0 0 1 * * certbot renew --quiet
   ```

## Troubleshooting

### Erro: "The certificate chain was issued by an authority that is not trusted"
- Você está usando certificado auto-assinado
- Use certificado de CA confiável

### Erro: "The specified network password is not correct"
- Verifique `CERTIFICATE_PASSWORD`
- Certificado pode não ter senha (use string vazia)

### Certificado não encontrado
- Verifique se `./certs/aspnetapp.pfx` existe
- Verifique permissões do arquivo
- Verifique o caminho no `docker-compose.production.yml`
