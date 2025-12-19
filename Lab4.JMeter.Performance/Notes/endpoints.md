# Selected Endpoints for Performance Testing

This document lists the stable GET endpoints selected for testing https://ffl.org.ua/

## Criteria
- Public pages (no login required)
- Stable URLs (not frequently changing)
- Realistic user navigation paths

## Selected Endpoints

1. **Home Page**
   - Title: ЛЬВІВСЬКА АСОЦІАЦІЯ ФУТБОЛУ
   - URL: https://ffl.org.ua/
   - Path: /

2. **News Page**
   - Title: Новини
   - URL: https://ffl.org.ua/news
   - Path: /news

3. **FAQ / Questions**
   - Title: Запитання до Асоціації
   - URL: https://ffl.org.ua/questions/faq
   - Path: /questions/faq

4. **Surveys / Questions**
   - Title: Опитування
   - URL: https://ffl.org.ua/questions
   - Path: /questions

5. **Contacts**
   - Title: Контакти
   - URL: https://ffl.org.ua/pages/66
   - Path: /pages/66

6. **Video Gallery**
   - Title: Відео
   - URL: https://ffl.org.ua/media/video
   - Path: /media/video

## Notes
- All endpoints use HTTPS protocol
- Domain: ffl.org.ua
- Most pages may redirect (301/302), so Follow Redirects must be enabled in JMeter

