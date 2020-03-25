-- 1. ts_vector
-- (1)
SELECT to_tsvector('The quick brown fox jumped over the lazy dog.');
-- (2) выполняем тот же запрос, но с другим текстом
SELECT to_tsvector('Съешь ещё этих мягких французских булок, да выпей чаю');

-- 2. tsquery
-- (1)
SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('fox');

SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('foxes');

SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('foxhound');
--(2)
SELECT to_tsvector('russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.') @@ to_tsquery('russian','булка');

SELECT ts_lexize('russian_stem', 'булок');

SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.') @@ to_tsquery('Russian','пирожки');
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских пирожков, да выпей чаю.') @@ to_tsquery('Russian','пирожок');

SELECT ts_lexize('russian_stem', 'пирожков');
SELECT ts_lexize('russian_stem', 'пирожки');
SELECT ts_lexize('russian_stem', 'пирожок');

-- 3. Операторы
--И
SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('fox & dog');
--ИЛИ
SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('fox | rat');
--отрицание
SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('!clown');
--группировка
SELECT to_tsvector('The quick brown fox jumped over the lazy dog') @@ to_tsquery('fox & (dog | rat) & !mice');

--И
SELECT to_tsvector('Полет на корабле "Восток" был сопряжен с огромными рисками для жизни Гагарина') 
		@@ to_tsquery('Russian', 'Гагарина & Восток');
--ИЛИ
SELECT to_tsvector('Полет на корабле "Восток" был сопряжен с огромными рисками для жизни Гагарина') 
		@@ to_tsquery('Russian', 'Гагарин | Королев');
--отрицание
SELECT to_tsvector('Полет на корабле "Восток" был сопряжен с огромными рисками для жизни Гагарина') 
		@@ to_tsquery('Russian', '!Гагарин');
--группировка
SELECT to_tsvector('Полет на корабле "Восток" был сопряжен с огромными рисками для жизни Гагарина') 
		@@ to_tsquery('Russian', 'полет & Гагарин | (Королев & !Байконур)');

-- 4. Поиск фраз
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.')
    @@ to_tsquery('Russian','мягких<2>булок');
-- модификация
SELECT to_tsvector('Russian', 'Съешь ещё этих мягких французских булок, да выпей чаю.')
    @@ to_tsquery('Russian','съешь<->ещё');

SELECT phraseto_tsquery('The Big Fat Rats');

-- 5. Утилиты
-- 1) ts_debug
SELECT * FROM ts_debug('english', 'We live on planet Earth');
SELECT * FROM ts_debug('russian', 'Мы живем на планете Земля');
-- 2) ts_headline
SELECT ts_headline('english', 'ts_headline accepts a document along with a query, and returns an excerpt from the document in which terms from the query are highlighted', to_tsquery('query & document'));
SELECT ts_headline('english', 'ts_headline принимает документ вместе с запросом и возвращает выдержку из документа, в которой выделяются слова из запроса', to_tsquery('запрос & документ'));


