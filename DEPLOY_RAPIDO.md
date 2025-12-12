# 🚀 Deploy Rápido no Portainer.io

## Método Mais Simples (Recomendado)

### 1. Preparar o Código no Git
```bash
# Se ainda não tem repositório Git
git init
git add .
git commit -m "Preparar para deploy"
git remote add origin <URL_DO_SEU_REPOSITORIO>
git push -u origin main
```

### 2. No Portainer.io

1. **Acesse o Portainer** → Menu **"Stacks"** → **"Add stack"**

2. **Configure:**
   - **Name**: `projeto-thalers`
   - **Build method**: **Repository**
   - **Repository URL**: Cole a URL do seu repositório Git
   - **Repository reference**: `main` (ou sua branch)
   - **Compose path**: `docker-compose.production.yml`

3. **Variáveis de Ambiente (Opcional):**
   ```
   POSTGRES_USER=postgres
   POSTGRES_PASSWORD=sua_senha_segura
   POSTGRES_DB=ThalersDb
   APP_PORT=8080
   ADMINER_PORT=8090
   ```

4. **Clique em "Deploy the stack"**

5. **Aguarde** o build e deploy (pode levar alguns minutos)

### 3. Acessar a Aplicação

- **Aplicação**: `http://seu-servidor:8080`
- **Adminer**: `http://seu-servidor:8090`

---

## ⚠️ Importante

- **Senhas**: Altere as senhas padrão em produção!
- **Firewall**: Configure o firewall para permitir as portas 8080 e 8090
- **HTTPS**: Considere usar um reverse proxy (Nginx/Traefik) para HTTPS

---

## 📝 Checklist Pré-Deploy

- [ ] Código commitado no Git
- [ ] `docker-compose.production.yml` criado
- [ ] Variáveis de ambiente configuradas
- [ ] Firewall configurado
- [ ] Senhas alteradas

---

Para mais detalhes, consulte o arquivo `PORTAINER_DEPLOY.md`

